using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : MonoBehaviour
{
    [SerializeField] float timeToShrink = 3f;
    [SerializeField] Vector3 finalScale = Vector3.zero;

    private void OnEnable()
    {
        Transform orig = transform;
        transform.LerpScale(finalScale, timeToShrink, this, ScaledToNothing);


    }

    void ScaledToNothing()
    {
        Destroy(gameObject);
    }
}
