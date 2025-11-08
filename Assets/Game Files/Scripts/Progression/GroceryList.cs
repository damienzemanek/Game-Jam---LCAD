using System;
using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;

[Serializable]
public class GroceryList : MonoBehaviour, IDependencyProvider
{
    [Provide] GroceryList Provide() => this;
    [SerializeReference] public List<GroceryItem> items;


}


[Serializable]
public class GroceryItem
{
    [SerializeField] bool have;
    [SerializeField] string _name;

    public string name { get => _name; set => _name = value; }

    public GroceryItem Copy()
    {
        GroceryItem newItem = new GroceryItem();
        newItem.name = name;
        newItem.have = have;
        return newItem;
    }
}