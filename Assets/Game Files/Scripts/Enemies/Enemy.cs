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
    [ShowInInspector] int maxCombosThisSpawn { get => type.combos.Count; }
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
    protected MonoBehaviour host;
    public EnemyType() { }

    [SerializeReference] public List<Combos> combos;
    [field: SerializeField] float health { get; set; }
    public abstract void StartCombo(Action postHook = null);

}

[Serializable]
public class Dragon : EnemyType
{
    public string name { get => "DRAGON"; }

    public Dragon() : base() { }

    public override void StartCombo(Action postHook = null)
    {
        combos.Rand().AttackLinear(postHook);
    }

}