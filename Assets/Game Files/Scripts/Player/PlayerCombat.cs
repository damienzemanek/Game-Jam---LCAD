using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(200)]
public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;

    [Inject] RepeatWorld world;
    [Inject] HitArea hitArea;
    [SerializeField] bool inCombat;
    [SerializeField] bool inDefence;



    [TitleGroup("Combat")]
    [SerializeField] GameObject[] combatDisplays;
    [SerializeField] Enemy enemy { get => world.currentEncounter.currentEnemy; }
    [SerializeField] public UnityEvent<float> onTakeDmgHook;
    [SerializeField] public UnityEvent onEnemyDefenceHook;
    [SerializeField] public UnityEvent onAttackHook;
    [SerializeField] public UnityEvent onDefencePhaseStart;
    [SerializeField] public UnityEvent onDefeatEnemy;




    [SerializeField] public GameObject destroyProjEffect;
    [ShowInInspector, ReadOnly] bool comboing = false;
    [ShowInInspector, ReadOnly] bool defending = false;
    [SerializeField] float delayBetweenPhases = 3f;
    [SerializeField] float dmg = 1f;

    [SerializeField] Slider _enemyHealthSlider;
    [SerializeField] TextMeshProUGUI enemyNameText;
    [SerializeField] GraphicRaycaster rayster;
    [SerializeField] EventSystem ev;

    public Slider enemyHealthSlider { get => _enemyHealthSlider; set => _enemyHealthSlider = value; }  

    private void Awake()
    {
        Instance = this;
        if(onTakeDmgHook == null) onTakeDmgHook = new();
        if(onAttackHook == null) onAttackHook = new();
        if(onEnemyDefenceHook == null) onEnemyDefenceHook = new();
        if(onDefencePhaseStart == null) onDefencePhaseStart = new();
        if(onDefeatEnemy == null) onDefeatEnemy = new();
        combatDisplays.SetAllActive(false);
    }

    private void OnEnable()
    {
        if (!world) this.Error("world null");
        if (world.startCombatHook == null) this.Error("combat hook null");
        if (world.startCombatHook.get == null) this.Error("get null");
        if (rayster == null) this.Error("Graphics raycster not set");
        if (enemyNameText == null) this.Error("Enemey name text null");
        if (ev == null) this.Error("event system null");

        world.startCombatHook?.get?.AddListener(EnterCombat);
    }

    private void OnDisable()
    {
        world.startCombatHook?.get?.RemoveListener(EnterCombat);
    }

    private void Update()
    {
        EnemyAttack();
        EnemyDefence();

        if (Input.GetKeyDown(KeyCode.Alpha0))
            if (enemy != null)
                KillEnemy();
    }

    public void EnterCombat()
    {
        if (inCombat) return;
        combatDisplays.SetAllActive(true);
        enemyNameText.text = enemy.type.name;
        ChangeToAttackPhase();
    }

    public void ExitCombat()
    {
        inCombat = false;
        inDefence = false;
        comboing = false;
        defending = false;
        StopAllCoroutines();
        combatDisplays.SetAllActive(false);
        hitArea.StopAllCoroutines();
    }

    [Button]
    public void KillEnemy()
    {
        world.currentEncounter.EnsureKillEnemy();
        onDefeatEnemy?.Invoke();
        ExitCombat();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Projectile") return;

        this.Log("Projectile hit player");
        onTakeDmgHook?.Invoke(other.TryGet<Projectile>().dmg);
        Destroy(other.gameObject);

    }



    #region Enemy Attack
    void ChangeToAttackPhase()
    {
        this.Log("Changing to attack phase");
        StopCoroutine(C_DefenceCycle());
        StartCoroutine(C_EnemeyAttackCycle());
    }

    void EnemyAttack()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

        Vector3 mousePos = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.transform.tag != "Projectile") return;
            hit.transform.TryGet<Projectile>().ClickedOn();
            onAttackHook?.Invoke();
            return;
        }
    }

    public IEnumerator C_EnemeyAttackCycle()
    {
        this.Log("C_AttackCycle Going");

        inCombat = true;
        inDefence = false;
        defending = false;
        enemy.currentCombo = 0;


        yield return new WaitForSeconds(1);

        while (inCombat)
        {
            this.Log($"ATTACKING, COMBO: {enemy.currentCombo}");

            comboing = true;
            enemy.StartCombo(
                () => this.DelayedCall(ChangeToDefencePhase, delayBetweenPhases), 
                () => comboing = false
                );
            while (comboing) yield return null;
        }
    }

    #endregion




    #region Enemy Defence

    void EnemyDefence()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

        PointerEventData pd = new PointerEventData(ev);
        pd.position = Input.mousePosition;
        var results = new List<RaycastResult>();

        rayster.Raycast(pd, results);

        GameObject hit = results.FirstOrDefault(r => r.gameObject.tag == "Defence").gameObject;
        if (hit == null) { this.Log("no hit"); return; }

        if (hit.gameObject.TryGet<HitAvaliable>().alreadyHit) return;

        enemy.TryGet<Health>().TakeDamage(dmg);
        hit.gameObject.TryGet<HitAvaliable>().Hit();
        onEnemyDefenceHook?.Invoke();
    }

    void ChangeToDefencePhase()
    {
        StopCoroutine(C_EnemeyAttackCycle());
        onDefencePhaseStart?.Invoke();
        StartCoroutine(C_DefenceCycle());
    }

    public IEnumerator C_DefenceCycle()
    {
        inCombat = false;
        comboing = false;
        inDefence = true;

        yield return new WaitForSeconds(1);

        while (inDefence)
        {
            this.Log("in defence");
            defending = true;
            hitArea.GenerateHitAreas(hitArea.TryGet<RectTransform>(), 
                () => {
                    ChangeToAttackPhase();
                    defending = false;
                });

            while (defending) yield return null;
            yield return new WaitForSeconds(1.25f);
        }
    }



    #endregion
}
