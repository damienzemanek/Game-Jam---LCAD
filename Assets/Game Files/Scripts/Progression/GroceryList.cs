using System;
using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;

[Serializable]
public class GroceryList : MonoBehaviour
{
    public static GroceryList Instance;
    
    [SerializeReference] public List<GroceryItem> items;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    public GroceryItem Copy()
    {
        GroceryItem newItem = new GroceryItem();
        newItem.type = type;
        newItem.have = have;
        return newItem;
    }
}