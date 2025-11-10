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
    [Inject] EnemyDb dungeonDB;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform completeItemSpawnPoint;
    [SerializeField, ReadOnly] Enemy _currentEnemy;

    public Enemy currentEnemy { get => _currentEnemy; set => _currentEnemy = value; }


    private void Start()
    {
        currentEnemy = null;
    }

    public void SpawnRandomEnemy()
    {
        currentEnemy = null;

        if (!dungeonDB) this.Error("No enemy db found");
        if (!spawnPoint) this.Error("Spawn point not set");
        this.Log("Spawning enemy");

        GameObject prefab = dungeonDB.GetEnemyPrefab();
        if (!prefab) this.Error("Did not get a prefab");

        currentEnemy = Instantiate(prefab, spawnPoint).TryGet<Enemy>();

    }

    public void SpawnDungeonCompleteItem()
    {
        currentEnemy = null;

        if (!dungeonDB) this.Error("No enemy db found");
        if (!spawnPoint) this.Error("Spawn point not set");
        this.Log("Spawning enemy");

        if (!dungeonDB.dungeonData.final)
        {
            GameObject prefab = dungeonDB.dungeonData.completeItem;
            if (!prefab) this.Error("Did not get a prefab");
            
            GameObject spawned = Instantiate(original: prefab, completeItemSpawnPoint);
            if(world.spawnedPickup != null) world.spawnedPickup = null;
            world.spawnedPickup = spawned;
        }
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
