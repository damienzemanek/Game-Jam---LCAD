using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartHook : MonoBehaviour
{
    [SerializeField] UnityEvent onStartHook;

    private void Awake()
    {
        if (onStartHook == null) onStartHook = new UnityEvent();
    }

    private void Start()
    {
        onStartHook.Invoke();
    }
}
