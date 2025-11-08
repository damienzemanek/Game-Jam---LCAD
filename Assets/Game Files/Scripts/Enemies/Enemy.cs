using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeReference] EnemyType type;
    [SerializeField] Animator animator;



    private void Awake()
    {
        animator = this.TryGet<Animator>();
    }

    public void Die(Action IsDeadHook)
    {
        animator.PlayWithHook("die", this, () =>
        {
            IsDeadHook?.Invoke();
            Destroy(gameObject);   
        });
    }


}


[Serializable]
public abstract class EnemyType
{
    [field: SerializeField] float health { get; set; }
    public abstract void Attack();

}

[Serializable]
public class Dragon : EnemyType
{
    public override void Attack()
    {

    }

}