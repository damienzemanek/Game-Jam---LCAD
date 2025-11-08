using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHook : MonoBehaviour
{
    public UnityEventPlus hook;

    public void Execute() => hook?.get?.Invoke();
    public void Execute(float delay) => hook?.InvokeWithDelay(this);

}
