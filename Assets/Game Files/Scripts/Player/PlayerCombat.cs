using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Inject] RepeatWorld world;
    [SerializeField] bool inCombat;

    public void EnterCombat()
    {
        if (inCombat) return;
        inCombat = true;
    }

    [Button]
    public void KillEnemy()
    {
        world.currentEncounter.EnsureKillEnemy();
    }
}
