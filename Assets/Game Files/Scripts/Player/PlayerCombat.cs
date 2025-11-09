using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(500)]
public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;

    [Inject] RepeatWorld world;
    [SerializeField] bool inCombat;
    [SerializeField] bool inDefence;


    [TitleGroup("Combat")]
    [SerializeField] GameObject combatDisplay;
    [SerializeField] Enemy enemy { get => world.currentEncounter.currentEnemy; }
    [SerializeField] public UnityEvent<float> onTakeDmgHook;
    [SerializeField] public GameObject destroyProjEffect;
    [ShowInInspector, ReadOnly] bool comboing = false;

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

        world.startCombatHook?.get?.AddListener(EnterCombat);
    }

    private void OnDisable()
    {
        world.startCombatHook?.get?.RemoveListener(EnterCombat);
    }

    private void Update()
    {
        PlayerAttack();
    }

    public void EnterCombat()
    {
        if (inCombat) return;
        inCombat = true;
        inDefence = false;
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
            enemy.StartCombo(ChangeToDefencePhase, () => comboing = false);
            while (comboing) yield return null;
            yield return new WaitForSeconds(0.25f);
        }
    }

    void ChangeToDefencePhase()
    {
        inCombat = false;
        comboing = false;
        inDefence = true;
        StopCoroutine(C_CombatCycle());

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
}
