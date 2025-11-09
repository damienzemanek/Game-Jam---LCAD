using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Extensions;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;

public class Enemy : MonoBehaviour
{
    [SerializeReference] EnemyType _type;
    [ShowInInspector] int maxCombosThisSpawn { get => (type.combos == null) ? 0 : type.combos.Count; }
    [SerializeField, ReadOnly] int _currentCombo;

    public int currentCombo { get => _currentCombo; set => _currentCombo = value; }


    public EnemyType type { get => _type; set => _type = value; } 

    Animator animator;

    private void Awake()
    {
        animator = this.TryGet<Animator>();
    }

    private void OnEnable()
    {
        AssignSelfAsHostToAttacks();
        currentCombo = 0;
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

    public void StartCombo(Action changeToDefencePhase, Action postHook = null)
    {
        if (currentCombo > maxCombosThisSpawn)
        {
            changeToDefencePhase?.Invoke();
            return;
        }

        type.StartCombo(() => postHook?.Invoke());
        currentCombo++;

    }



}


    [Serializable]
public abstract class EnemyType
{
    public abstract string name { get; }

    protected MonoBehaviour host;
    public EnemyType() { }

    [SerializeReference] public List<Combos> combos;
    [field: SerializeField] float health { get; set; }
    public abstract void StartCombo(Action postHook = null);

}

[Serializable]
public class Dragon : EnemyType
{
    public override string name { get => "DRAGON"; }

    public Dragon() : base() { }

    public override void StartCombo(Action postHook = null)
    {
        combos.Rand().AttackLinear(postHook);
    }



}

[Serializable]
public class GreenDragon : EnemyType
{
    public override string name { get => "DRAGON"; }

    public GreenDragon() : base() { }

    public override void StartCombo(Action postHook = null)
    {
        combos.Rand().AttackLinear(postHook);
    }
}


[Serializable]
public class Knight : EnemyType
{
    public override string name { get => "KNIGHT"; }

    public Knight() : base() { }

    public override void StartCombo(Action postHook = null)
    {
        combos.Rand().AttackLinear(postHook);
    }

}

[Serializable]
public class SkeletonArcher : EnemyType
{
    public override string name { get => "SKELETON ARCHER"; }

    public SkeletonArcher() : base() { }

    public override void StartCombo(Action postHook = null)
    {
        combos.Rand().AttackLinear(postHook);
    }

}

[Serializable]
public class SkeletonWarrior : EnemyType
{
    public override string name { get => "SKELETON WARRIOR"; }

    public SkeletonWarrior() : base() { }

    public override void StartCombo(Action postHook = null)
    {
        combos.Rand().AttackLinear(postHook);
    }

}

[Serializable]
public class KnightCaptain : EnemyType
{
    public override string name { get => "CAPTAIN"; }

    public KnightCaptain() : base() { }

    public override void StartCombo(Action postHook = null)
    {
        combos.Rand().AttackLinear(postHook);
    }

}

[Serializable]
public class CashRegisterEmployeeBoss : EnemyType
{
    public override string name => "<color=#FF0000>CASHIER</color>";

    public CashRegisterEmployeeBoss() : base() { }

    public override void StartCombo(Action postHook = null)
    {
        combos.Rand().AttackLinear(postHook);
    }

}