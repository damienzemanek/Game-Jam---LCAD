using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using DesignPatterns.CreationalPatterns;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyDb : MonoBehaviour, IDependencyProvider
{
    [Provide] EnemyDb Provide() => this;

    [SerializeField] bool linear;
    [SerializeField, ShowIf("linear")] public int currentEnemy; 

    public List<GameObject> enemyPrefabs;
    public GameObject completeItemPrefab;

    private void Start()
    {
        currentEnemy = 0;
        if (enemyPrefabs == null) this.Error("Enemy db needs prefabs, set them");
    }

    public GameObject GetEnemyPrefab()
    {
        if (!linear)
            return enemyPrefabs.Rand();

        if (enemyPrefabs.Count == 0) { this.Error("Enemy list empty"); return null; }

        if (currentEnemy >= enemyPrefabs.Count)
            currentEnemy = 0;

        return enemyPrefabs[currentEnemy++];
    }
}
