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
        EnsureListRef();
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

    [Sirenix.OdinInspector.Button, ExecuteAlways]
    public void LoadGroceryList(bool createIfMissing = true)
    {
        EnsureListRef();

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
    }

    void EnsureListRef()
    {
        if (persistanceReferenceToList != null) return;
        persistanceReferenceToList = FindFirstObjectByType<GroceryList>(FindObjectsInactive.Include);
        if (persistanceReferenceToList == null)
            persistanceReferenceToList = new GameObject("GroceryList").AddComponent<GroceryList>();
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



