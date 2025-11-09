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

    [SerializeField] bool transform_;
    [SerializeField] bool vector3;

    [SerializeField, ShowIf("transform_")] Transform origPos;
    [SerializeField, ShowIf("transform_")] Transform movedPos;



    [SerializeField, ShowIf("vector3"), ReadOnly] Vector3 origVec3Pos;
    [SerializeField, ShowIf("vector3")] Vector3 newVec3Pos;

    private void Awake()
    {
        origVec3Pos = transform.localPosition;
    }

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

        if (vector3)
        {
            objToMove.transform.Lerp(origVec3Pos + newVec3Pos, duration, mono: this);
            return;
        }
        
        objToMove.transform.Lerp(movedPos.position, duration, mono: this);
    }

    void MoveBack()
    {
        if (vector3)
        {
            objToMove.transform.Lerp(origVec3Pos, duration, mono: this);
            return;
        }
        objToMove.transform.Lerp(origPos.position, duration, this);
        this.Log("moving");
    }
}
