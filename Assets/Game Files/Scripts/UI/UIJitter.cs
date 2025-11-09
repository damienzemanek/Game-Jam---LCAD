using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIJitter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float duration;
    [SerializeField] bool onMouseOver = true;

    [SerializeField, ReadOnly] Vector3 initialSize;
    [SerializeField] Vector3 sizeJitterIncrease;

    [SerializeField, ReadOnly] Quaternion initialRot;
    [SerializeField] Quaternion rotTo;

    private void Awake()
    {
        initialSize = transform.localScale;
        initialRot = transform.rotation;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(onMouseOver)
            Jitter();
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (onMouseOver)
            ResetJitter();
    }


    public void Jitter()
    {
        StopAllCoroutines();
        transform.LerpScale(sizeJitterIncrease, duration, this);
        transform.LerpRot(rotTo, duration, this);
    }

    public void ResetJitter()
    {
        StopAllCoroutines();
        transform.LerpScale(initialSize, duration, this);
        transform.LerpRot(initialRot, duration, this);

    }

    [Button]
    public void JitterThenReset()
    {
        transform.LerpScale(sizeJitterIncrease, duration, this);
        transform.LerpRot(rotTo, duration, this, ResetJitter);
    }

}
