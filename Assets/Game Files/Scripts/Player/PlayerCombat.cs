using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

[DefaultExecutionOrder(500)]
public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;

    [Inject] RepeatWorld world;
    [SerializeField] bool inCombat;

    [TitleGroup("Combat")]
    [SerializeField] GameObject combatDisplay;
    [SerializeField] Enemy enemy { get => world.currentEncounter.currentEnemy; }

    private void Awake()
    {
        Instance = this;
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

    public void EnterCombat()
    {
        if (inCombat) return;
        inCombat = true;

        StartCoroutine(C_CombatCycle());
    }

    public void ExitCombat()
    {
        inCombat = false;
    }

    public IEnumerator C_CombatCycle()
    {
        while (inCombat)
        {
            yield return new WaitForSeconds(1);
            enemy.type.StartCombo();
        }
    }

    [Button]
    public void KillEnemy()
    {
        world.currentEncounter.EnsureKillEnemy();
        ExitCombat();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Projectile")) return;
    }
}
