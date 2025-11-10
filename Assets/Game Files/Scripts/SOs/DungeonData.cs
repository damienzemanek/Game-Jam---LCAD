using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "Dungeon Data", menuName = "ScriptableObjects/DungeonData")]
public class DungeonData : ScriptableObject
{
    bool notFinal { get => !final; }
    [SerializeField] public bool final;
    [SerializeField] public bool linear = true;
    [SerializeField] public int finalSceneIndex;
    [SerializeField] public AudioClip bgMusic;

    public List<GameObject> enemyPrefabs;
    [ShowIf("notFinal")] public GameObject completeItem;
    [ShowIf("notFinal")] public GroceryItem.ItemType giveType;
}
