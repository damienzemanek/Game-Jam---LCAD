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
        LoadGroceryList();
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

    [Button, ExecuteAlways]
    public void LoadGroceryList()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        print(message: "a");

        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<SaveData>(json);

        if (data?.items == null || data.items == null)
        {
            Debug.LogWarning("Save file had no items.");
            return;
        }

        if (persistanceReferenceToList == null)
            persistanceReferenceToList = new GroceryList();

        persistanceReferenceToList.items ??= new List<GroceryItem>();
        persistanceReferenceToList.items.Clear();
        persistanceReferenceToList.items.AddRange(data.items);

        Debug.Log($"Loaded {persistanceReferenceToList.items.Count} items from {path}");


    }

    public void OnApplicationQuit()
    {
        SaveGroceryList();
    }
}



