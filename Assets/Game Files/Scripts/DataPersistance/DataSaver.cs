using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public static DataSaver Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        persistanceReferenceToList = FindAnyObjectByType<GroceryList>();
    }

    public GroceryList persistanceReferenceToList;

    [Serializable]
    public class SaveData
    {
        public List<GroceryItem> items;
    }

    [Button, ExecuteAlways]
    public void SaveGroceryList()
    {
        if (persistanceReferenceToList == null) this.Error("No grocery list is assigned");


        SaveData data = new SaveData();

        data.items = persistanceReferenceToList.items
            .Select(i => i.Copy())
            .ToList();

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void SaveGroceryListManual(List<GroceryItem> items)
    {
        if (persistanceReferenceToList == null) this.Error("No grocery list is assigned");


        SaveData data = new SaveData();

        data.items = items
            .Select(i => i.Copy())
            .ToList();

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    [Sirenix.OdinInspector.Button, ExecuteAlways]
    public void LoadGroceryList(bool createIfMissing = true)
    {
        string path = Path.Combine(Application.persistentDataPath, "savefile.json");

        if (!File.Exists(path))
        {
            if (createIfMissing)
            {
                persistanceReferenceToList.items = GetDefaultItems();
                SaveGroceryList(); 
            }
            return;
        }

        var json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<SaveData>(json);

        persistanceReferenceToList.items = (data?.items != null && data.items.Count > 0)
            ? data.items
            : GetDefaultItems();

        Debug.Log($"Loaded {persistanceReferenceToList.items.Count} items from {path}");
        persistanceReferenceToList.items.PrintList(i => i.type);
    }

    public bool hasSaveFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "savefile.json");
        return File.Exists(path);
    }

    public void InitializeData(GroceryList groceryList)
    {
        groceryList.items = GetDefaultItems();
        SaveGroceryListManual(groceryList.items);
        groceryList.items.PrintList(i => i.type);
    }

    static List<GroceryItem> GetDefaultItems() => new List<GroceryItem>
    {
        new GroceryItem(GroceryItem.ItemType.Apple),
        new GroceryItem(GroceryItem.ItemType.Cereal),
        new GroceryItem(GroceryItem.ItemType.Soda)
    };



    public void OnApplicationQuit()
    {
        SaveGroceryList();
    }
}



