using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class FlashColor : MonoBehaviour
{
    [SerializeField] Image panel;
    [SerializeField] Color color;
    [SerializeField] float flashInTime = 0.06f;
    [SerializeField] float flashOutTime = 0.16f;

    [Button, ExecuteAlways]
    void Flash()
    {
        StartCoroutine(C_Fade(() => StartCoroutine(C_FadeToVisible())));
    }

    IEnumerator C_Fade(Action? posthook = null)
    {
        float t = 0;
        while (t < 0.99f)
        {
            t += Time.deltaTime / flashInTime;
            yield return null;
 
        }
;
    }

    IEnumerator C_FadeToVisible(Action? posthook = null)
    {
        Color fade = color;
        fade.a = 1;
        float increment = 1;
        while (increment > 0.05f)
        {

            yield return null;
            fade.a = increment;
            panel.color = fade;
        }
        fade.a = 0;
        panel.color = fade;
        posthook?.Invoke();
    }
}
