using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;


public class Encounter : RuntimeInjectableMonoBehaviour
{
    [Inject] RepeatWorld world;
    [Inject] EnemyDb enemies;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform completeItemSpawnPoint;
    [SerializeField, ReadOnly] Enemy _currentEnemy;

    public Enemy currentEnemy { get => _currentEnemy; set => currentEnemy = value; }


    private void Awake()
    {
        currentEnemy = null;
    }

    public void SpawnRandomEnemy()
    {
        currentEnemy = null;

        if (!enemies) this.Error("No enemy db found");
        if (!spawnPoint) this.Error("Spawn point not set");
        this.Log("Spawning enemy");

        GameObject prefab = enemies.GetRandEnemyPrefab();
        if (!prefab) this.Error("Did not get a prefab");

        currentEnemy = Instantiate(prefab, spawnPoint).TryGet<Enemy>();

    }

    public void SpawnDungeonCompleteItem()
    {
        currentEnemy = null;

        if (!enemies) this.Error("No enemy db found");
        if (!spawnPoint) this.Error("Spawn point not set");
        this.Log("Spawning enemy");

        GameObject prefab = enemies.completeItemPrefab;
        if (!prefab) this.Error("Did not get a prefab");
        Instantiate(original: prefab, completeItemSpawnPoint);
    }

    public void EnsureKillEnemy()
    {
        currentEnemy.Die(FinishedEncounter);
    }

    public void FinishedEncounter()
    {
        world.TransitionToNextEncounter();
        Destroy(currentEnemy);
    }
}
