using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float duration;
    [SerializeField] GameObject objToMove;

    [SerializeField] Transform origPos;
    [SerializeField] Transform movedPos;


    public void OnPointerEnter(PointerEventData data)
    {
        MoveTo();
    }

    public void OnPointerExit(PointerEventData data)
    {
        MoveBack();
    }


    void MoveTo()
    {
        StopAllCoroutines();
        objToMove.transform.Lerp(movedPos.position, duration, this);
    }

    void MoveBack()
    {
        objToMove.transform.Lerp(origPos.position, duration, this);
        this.Log("moving");
    }
}
