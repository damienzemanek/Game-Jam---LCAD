using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quitter : MonoBehaviour
{
    public void Quit()
    {
        DataSaver.Instance.SaveGroceryList();
        Application.Quit();
    }
}
