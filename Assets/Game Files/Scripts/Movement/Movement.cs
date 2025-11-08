using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour, IDependencyProvider
{
    [Provide] Movement Provide() => this;
    [SerializeField] GameObject player;

    public UnityEvent onMoveHook;

    private void Awake()
    {
        if(onMoveHook == null) onMoveHook = new UnityEvent();
    }


}
