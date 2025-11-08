using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProgressionUpdater : MonoBehaviour
{
    GroceryList list;
    DataSaver saver;
    [SerializeField] GroceryItem.ItemType type;

    private void Awake()
    {
        list = FindAnyObjectByType<GroceryList>();
        saver = FindAnyObjectByType<DataSaver>();
    }

    [Button]
    public void UpdateProgressionWithItem()
    {
        GroceryItem match = list.items.FirstOrDefault(i => i.type == type);
        if (match != null)
        {
            match.have = true;
            this.Log($"Updated {match.type} to {match.have}");

        }
        saver.SaveGroceryList();

    }
}
