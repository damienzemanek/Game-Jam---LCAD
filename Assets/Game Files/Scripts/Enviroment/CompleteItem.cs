using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.VFX;

public class CompleteItem : MonoBehaviour
{
    [SerializeField] GameObject pickupVFX;

    private void OnEnable()
    {
        transform.Children().SetAllActive(true);
        pickupVFX.SetActive(false);
    }

    public void Grab()
    {
        this.Log("GRABBING");
        transform.Children().SetAllActive(false);
        pickupVFX.SetActive(true);
        pickupVFX.TryGet<VisualEffect>().Play();
    }
}
