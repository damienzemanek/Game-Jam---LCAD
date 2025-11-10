using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "Dungeon Data", menuName = "ScriptableObjects/DungeonData")]
public class DungeonData : ScriptableObject
{
    public bool final;
    public List<GameObject> enemyPrefabs;
    public GameObject completeItem;
    public GroceryItem.ItemType giveType;
}
