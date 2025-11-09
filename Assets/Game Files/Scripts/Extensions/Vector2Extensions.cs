using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 GetRandomPointOnImage(this RectTransform rect)
    {
        Vector2 size = rect.rect.size;
        size = size / 2;
        float x = Random.Range(-size.x, size.x);
        float y = Random.Range(-size.y, size.y);
        return rect.TransformPoint(new Vector3(x, y, 0f));
    }
}
