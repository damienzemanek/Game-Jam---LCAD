using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using UnityEngine;

[Serializable]
public class GroceryList : MonoBehaviour
{
    public static GroceryList Instance;
    
    [SerializeReference] public List<GroceryItem> items;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else if (Instance != this) { Destroy(gameObject); return; }

    }

    public bool hasAllItems()
    {
        return items != null && items.Count > 0 && items.All(i => i.have);
    }

}


[Serializable]
public class GroceryItem
{
    public enum ItemType
    {
        Apple,
        Bread,
        Soup,
        Cereal,
        Soda
    }

    [SerializeField] bool _have;
    [SerializeField] ItemType _type;

    public bool have { get => _have ; set => _have = value; }
    public ItemType type { get => _type; set => _type = value; }

    public GroceryItem() { }

    public GroceryItem(ItemType type)
    {
        _type = type;
    }

    public GroceryItem Copy()
    {
        GroceryItem newItem = new GroceryItem(type);
        newItem.type = type;
        newItem.have = have;
        return newItem;
    }
}