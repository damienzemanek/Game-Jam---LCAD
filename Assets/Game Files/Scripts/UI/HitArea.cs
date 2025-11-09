using System;
using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class HitArea : MonoBehaviour, IDependencyProvider
{
    [Provide] HitArea Provide() => this; 
    [SerializeField] GameObject hitAreaIndividualPrefab;
    [SerializeField] Vector2 hitAreaCount = new Vector2(3, 5);


    [Button]
    public void GenerateHitAreas(RectTransform rect, Action postHook = null)
        => StartCoroutine(C_GenerateHitAreas(rect));
    IEnumerator C_GenerateHitAreas(RectTransform rect, Action postHook = null)
    {
        List<GameObject> hitAreas = new List<GameObject>();

        float count = hitAreaCount.Rand();

        for (int i = 0; i < count; i++)
        {
            Vector2 pos = rect.GetRandomPointOnImage();

            GameObject newHitArea = Instantiate(
                hitAreaIndividualPrefab,
                pos,
                Quaternion.identity,
                transform
                );

            hitAreas.Add(newHitArea);

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        postHook?.Invoke();
    }
}
