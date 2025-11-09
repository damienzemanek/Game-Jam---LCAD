using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckProgress : MonoBehaviour
{
    public static CheckProgress Instance;

    private void Awake()
    {
        Instance = this;
        icon.SetActive(false);
        button.interactable = false;
    }

    [SerializeField] Button button;
    [SerializeField] GameObject icon;


    public void Unlock()
    {
        button.interactable = true;
        icon.SetActive(false);
    }

    public void Lock()
    {
        button.interactable = false;
        icon.SetActive(true);
    }
}
