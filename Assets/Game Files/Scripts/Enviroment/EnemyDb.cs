using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using DesignPatterns.CreationalPatterns;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyDb : MonoBehaviour, IDependencyProvider
{
    GroceryList list;
    [SerializeField] int dataIndex;
    [SerializeReference] public DungeonData dungeonData;
    
    [Provide] EnemyDb Provide() => this;

    [SerializeField] bool linear;
    [SerializeField, ShowIf("linear")] public int currentEnemy;

    private void Start()
    {
        list = GroceryList.Instance;
        currentEnemy = 0;
        if (dungeonData.enemyPrefabs == null) this.Error("Enemy db needs prefabs, set them");
    }

    public GameObject GetEnemyPrefab()
    {
        if (!linear)
            return dungeonData.enemyPrefabs.Rand().gameObject;

        if (dungeonData.enemyPrefabs.Count == 0) { this.Error("Enemy list empty"); return null; }

        if (currentEnemy >= dungeonData.enemyPrefabs.Count)
            currentEnemy = 0;

        return dungeonData.enemyPrefabs[currentEnemy++].gameObject;
    }

    [Button]
    public void UpdateProgressionWithItem()
    {
        GroceryItem match = list.items.FirstOrDefault(i => i.type == dungeonData.giveType);
        if (match != null)
        {
            match.have = true;
            this.Log($"Updated {match.type} to {match.have}");
        }

    }
}
