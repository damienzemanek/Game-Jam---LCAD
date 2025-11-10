using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCaller : MonoBehaviour
{
    public void SaveGame()
    {
        DataSaver.Instance.SaveGroceryList();
    }
}
