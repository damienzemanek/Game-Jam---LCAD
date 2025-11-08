using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeReference] EnemyType _type;

    public EnemyType type { get => _type; set => _type = value; } 

    Animator animator;

    private void Awake()
    {
        animator = this.TryGet<Animator>();
    }

    private void OnEnable()
    {
        AssignSelfAsHostToAttacks();
    }

    public void AssignSelfAsHostToAttacks()
    {
        type.combos.ForEach(c => c.Init(this));
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
    protected MonoBehaviour host;
    public EnemyType() { }

    [SerializeReference] public List<Combos> combos;
    [field: SerializeField] float health { get; set; }
    public abstract void StartCombo();

}

[Serializable]
public class Dragon : EnemyType
{
    public Dragon() : base() { }

    public override void StartCombo()
    {
        combos.Rand().AttackLinear();
    }

}