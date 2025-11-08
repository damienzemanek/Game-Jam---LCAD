using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using DesignPatterns.CreationalPatterns;
using Extensions;
using UnityEngine;

public class EnemyDb : MonoBehaviour, IDependencyProvider
{
    [Provide] EnemyDb Provide() => this;

    public List<GameObject> enemyPrefabs;
    public GameObject completeItemPrefab;

    private void Start()
    {
        if (enemyPrefabs == null) this.Error("Enemy db needs prefabs, set them");
    }

    public GameObject GetRandEnemyPrefab() => enemyPrefabs.Rand();
}
