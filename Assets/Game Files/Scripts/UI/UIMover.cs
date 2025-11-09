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

    [SerializeField] bool onMouseOver = true;
    [SerializeField] bool destroyAfter = false;
    [SerializeField] bool transform_;
    [SerializeField] bool vector3;

    [SerializeField, ShowIf("transform_")] Transform origPos;
    [SerializeField, ShowIf("transform_")] Transform movedPos;



    [SerializeField, ShowIf("vector3"), ReadOnly] Vector3 origVec3Pos;
    [SerializeField, ShowIf("vector3")] Vector3 offset;

    private void Awake()
    {
        origVec3Pos = objToMove.transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(onMouseOver) 
            MoveTo();
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (onMouseOver)
            MoveBack();
    }


    public void MoveTo()
    {
        StopAllCoroutines();

        if (vector3)
        {
            Vector3 newPos = origVec3Pos + offset;
            Vector3 newPosWorld = objToMove.transform.parent 
                ? objToMove.transform.parent.TransformPoint(newPos)
                : newPos;

            objToMove.transform.Lerp(newPosWorld, duration, mono: this, DoDestroy);
            return;
        }
        
        objToMove.transform.Lerp(movedPos.position, duration, mono: this, DoDestroy);
    }

    public void MoveBack()
    {
        if (vector3)
        {
            objToMove.transform.Lerp(origVec3Pos, duration, mono: this, DoDestroy);
            return;
        }
        objToMove.transform.Lerp(origPos.position, duration, this, DoDestroy);
        this.Log("moving");
    }

    void DoDestroy()
    {
        if(destroyAfter)
            Destroy(gameObject);
    }

}
