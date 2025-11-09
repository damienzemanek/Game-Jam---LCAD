using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveForwardAndBack : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float stayDuration;
    [SerializeField] GameObject objToMove;
    [SerializeField] Transform origPos;
    [SerializeField] Transform movedPos;

    public void MoveBackAndForth()
    {
        objToMove.transform.Lerp(
            movedPos.position,
            duration, 
            this,
            () => this.DelayedCall(MoveBack, stayDuration)
            );
    }

    public void MoveTo()
    {
        StopAllCoroutines();
        objToMove.transform.Lerp(movedPos.position, duration, this);
    }

    public void MoveBack()
    {
        objToMove.transform.Lerp(origPos.position, duration, this);
        this.Log("moving");
    }

}
