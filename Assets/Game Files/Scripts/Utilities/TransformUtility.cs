using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtility 
{
    public static void Slerp(this Transform myTransform, Transform to, float duration, MonoBehaviour mono)
        => mono.StartCoroutine(C_Slerp(myTransform, to, duration));

    public static IEnumerator C_Slerp(this Transform myTransform, Transform to, float duration)
    {
        Vector3 start = myTransform.position;
        Vector3 end = to.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            myTransform.position = Vector3.Slerp(start, end, t);
            yield return null;
        }

        myTransform.position = end;
    }

    public static void Slerp(this Transform myTransform, Vector3 to, float duration, MonoBehaviour mono)
        => mono.StartCoroutine(C_Slerp(myTransform, to, duration));

    public static IEnumerator C_Slerp(this Transform myTransform, Vector3 to, float duration)
    {
        Vector3 start = myTransform.position;
        Vector3 end = to;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            myTransform.position = Vector3.Slerp(start, end, t);
            yield return null;
        }

        myTransform.position = end;
    }
}
