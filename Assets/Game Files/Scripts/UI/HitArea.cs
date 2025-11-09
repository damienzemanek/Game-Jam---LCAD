using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    [SerializeField] GameObject hitAreaIndividualPrefab;
    [SerializeField] Vector2 hitAreaCount = new Vector2(3, 5);

    [Button]
    public void GenerateHitAreas(RectTransform rect)
        => StartCoroutine(C_GenerateHitAreas(rect));
    IEnumerator C_GenerateHitAreas(RectTransform rect)
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
        
    }
}
