using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnActive : MonoBehaviour
{
    public Transform origPos;
    public Transform finalPos;
    [SerializeField] float duration = 0.7f;

    private void OnEnable() => Move();

    void Move()
    {
        transform.position = origPos.position;
        transform.Lerp(finalPos.position, duration, this);
    }
}
