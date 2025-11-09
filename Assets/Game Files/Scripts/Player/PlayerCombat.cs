using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(500)]
public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;

    [Inject] RepeatWorld world;
    [Inject] HitArea hitArea;
    [SerializeField] bool inCombat;
    [SerializeField] bool inDefence;


    [TitleGroup("Combat")]
    [SerializeField] GameObject combatDisplay;
    [SerializeField] Enemy enemy { get => world.currentEncounter.currentEnemy; }
    [SerializeField] public UnityEvent<float> onTakeDmgHook;
    [SerializeField] public GameObject destroyProjEffect;
    [ShowInInspector, ReadOnly] bool comboing = false;
    [ShowInInspector, ReadOnly] bool defending = false;
    [SerializeField] float delayBetweenPhases = 3f;

    [SerializeField] GraphicRaycaster rayster;
    [SerializeField] EventSystem ev;

    private void Awake()
    {
        Instance = this;
        if(onTakeDmgHook == null) onTakeDmgHook = new();
        combatDisplay.SetActive(false);
    }

    private void OnEnable()
    {
        if (!world) this.Error("world null");
        if (world.startCombatHook == null) this.Error("combat hook null");
        if (world.startCombatHook.get == null) this.Error("get null");
        if (rayster == null) this.Error("Graphics raycster not set");

        world.startCombatHook?.get?.AddListener(EnterCombat);
    }

    private void OnDisable()
    {
        world.startCombatHook?.get?.RemoveListener(EnterCombat);
    }

    private void Update()
    {
        if (inCombat)
            PlayerAttack();
        else if (inDefence)
            EnemyDefence();
    }

    public void EnterCombat()
    {
        if (inCombat) return;
        inCombat = true;
        inDefence = false;
        defending = false;
        combatDisplay.SetActive(true);


        StartCoroutine(C_CombatCycle());
    }

    public void ExitCombat()
    {
        inCombat = false;
        combatDisplay.SetActive(false);
    }

    public IEnumerator C_CombatCycle()
    {
        yield return new WaitForSeconds(1);

        while (inCombat)
        {
            comboing = true;
            enemy.StartCombo(
                () => this.DelayedCall(ChangeToDefencePhase, delayBetweenPhases), 
                () => comboing = false
                );
            while (comboing) yield return null;
        }
    }

    void ChangeToDefencePhase()
    {
        inCombat = false;
        comboing = false;
        inDefence = true;
        StopCoroutine(C_CombatCycle());
        StartCoroutine(C_DefenceCycle());

    }

    public IEnumerator C_DefenceCycle()
    {
        yield return new WaitForSeconds(1);

        while (inDefence)
        {
            defending = true;
            hitArea.GenerateHitAreas(hitArea.TryGet<RectTransform>(), StopEnemyDefending);
            while (defending) yield return null;
            yield return new WaitForSeconds(0.25f);
        }
    }

    void StopEnemyDefending()
    {
        inDefence = false;
        defending = false;
        StopCoroutine(C_DefenceCycle());
    }

    [Button]
    public void KillEnemy()
    {
        world.currentEncounter.EnsureKillEnemy();
        ExitCombat();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Projectile") return;

        this.Log("Projectile hit player");
        onTakeDmgHook?.Invoke(other.TryGet<Projectile>().dmg);
        Destroy(other.gameObject);

    }

    void PlayerAttack()
    {
        if (!Input.GetKey(KeyCode.Mouse0)) return;

        Vector3 mousePos = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.transform.tag != "Projectile") return;
            hit.transform.TryGet<Projectile>().ClickedOn();
        }
    }

    void EnemyDefence()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

        PointerEventData pd = new PointerEventData(ev);
        pd.position = Input.mousePosition;
        var results = new List<RaycastResult>();

        rayster.Raycast(pd, results);

        GameObject hit = results.FirstOrDefault(r => r.gameObject.tag == "Defence").gameObject;
        if (hit == null) { this.Log("no hit"); return; }

        Destroy(hit.gameObject);
        
    }
}
