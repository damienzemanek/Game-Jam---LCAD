using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HitAvaliable : MonoBehaviour
{
    [SerializeField] float timeToFade = 0.5f;
    [SerializeField] public bool alreadyHit = false;
    [SerializeField] Image img;
    [SerializeField] Sprite changeToCut;
    [SerializeField] UnityEvent cutHook;

    private void Awake()
    {
        alreadyHit = false;
        if (cutHook == null) cutHook = new();
    }

    public void Hit()
    {
        alreadyHit = true;
        cutHook?.Invoke();
        img.sprite = changeToCut;
        StartCoroutine(C_Hit());
    }

    IEnumerator C_Hit()
    {
        Color full = img.color;
        full.a = 1f;
        img.color = full;


        float t = 0f;


        while(t < 1f)
        {
            t += Time.deltaTime / timeToFade;
            full.a = 1f - t;     
            img.color = full;
            yield return null;
        }

        full.a = 0f;
        img.color = full;
    }
}
