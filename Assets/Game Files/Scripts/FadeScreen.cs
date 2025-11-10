using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public bool isFading = false;
    public Image panel;
    public float incrementDelay = 0.03f;
    public float fadeStep = 0.02f;

    [SerializeField] UnityEvent localPostHook;

    private void Awake()
    {
        if(localPostHook == null) localPostHook = new UnityEvent();
    }

    public void FadeToFullyOpaque()
        => StartCoroutine(C_FadeToFullyOpaque(() => localPostHook?.Invoke()));

    public void FadeToFullyTransparent()
        => StartCoroutine(routine: C_FadeToFullyTransparent(() => localPostHook?.Invoke()));

    [Button]
    public void FadeToFullyOpaque(Action postHook = null)
        => StartCoroutine(C_FadeToFullyOpaque(postHook));

    [Button]
    public void FadeToFullyTransparent(Action postHook = null)
        => StartCoroutine(routine: C_FadeToFullyTransparent(postHook));

    public void FadeInAndOut() => StartCoroutine(C_FadeInAndOut());

    public void FadeInAndOutCallback(Action? prehook = null, Action? midhook = null, Action? posthook = null, float? blackScreenTime = 0f)
            => StartCoroutine(routine: C_FadeInAndOut(prehook, midhook, posthook, blackScreenTime));

    IEnumerator C_FadeInAndOut(Action? prehook = null, Action? midhook = null, Action? posthook = null, float? blackScreenTime = 0f)
    {
        if (isFading) yield break;
        prehook?.Invoke();

        yield return StartCoroutine(C_FadeToFullyOpaque(midhook));

        float fadeDuration = (1f / fadeStep) * incrementDelay;
        yield return new WaitForSeconds(fadeDuration * 0.5f);

        if(blackScreenTime > 0f) yield return new WaitForSeconds((float)blackScreenTime);

        yield return StartCoroutine(C_FadeToFullyTransparent(posthook));
    }



    IEnumerator C_FadeToFullyOpaque(Action? posthook = null)
    {
        if (isFading) yield break;
        isFading = true;
        panel.gameObject.SetActive(true);

        Color fade = panel.color;
        fade.a = 0;
        panel.color = fade;

        float increment = 0;
        while (increment < 0.95f)
        {
            increment += fadeStep;
            yield return new WaitForSeconds(incrementDelay);
            fade.a = increment;
            panel.color = fade;
        }
        fade.a = 1;
        panel.color = fade;
        isFading = false;
        posthook?.Invoke();
    }

    IEnumerator C_FadeToFullyTransparent(Action? posthook = null)
    {
        if (isFading) yield break;
        isFading = true;
        panel.gameObject.SetActive(true);

        Color fade = panel.color;
        fade.a = 1;
        panel.color = fade;

        float increment = 1;
        while (increment > 0.05f)
        {
            increment -= fadeStep;
            yield return new WaitForSeconds(incrementDelay);
            fade.a = increment;
            panel.color = fade;
        }
        fade.a = 0;
        panel.color = fade;
        isFading = false;
        posthook?.Invoke();
    }
}