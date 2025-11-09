using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : MonoBehaviour
{
    [SerializeField] Vector3 finalScale = Vector3.zero;

    private void OnEnable()
    {
        Transform orig = transform;
        transform.LerpScale(finalScale, 4f, this, ScaledToNothing);


    }

    void ScaledToNothing()
    {
        Destroy(gameObject);
    }
}
