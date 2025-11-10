using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DisplayProgress : MonoBehaviour
{
    GroceryList list;

    [SerializeField] List<TextMeshProUGUI> itemTexts;
    private void Start()
    {
        list = GroceryList.Instance;
        if (list == null) this.Error("did not find grocery list item storage");

        DisplayChanges();
    }

    [Button]
    void DisplayChanges()
    {
        for(int i = 0; i < list.items.Count; i++)
        {
            GroceryItem item = list.items[i];
            TextMeshProUGUI text = itemTexts[i];


            if (item.have) text.text = $"<s>{item.type.ToString()}</s>";
            else text.text = item.type.ToString();
        }
    }
}
