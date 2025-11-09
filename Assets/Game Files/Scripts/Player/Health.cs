using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected UnityEventPlus onDeathHook;

    private void Awake()
    {
        if (onDeathHook == null) onDeathHook = new UnityEventPlus();
        maxHp = currentHp;
    }


    [Button]
    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        if (currentHp < 0)
        {
            onDeathHook?.get?.Invoke();
            Die();
        }
    }

    public abstract void Die();
}
