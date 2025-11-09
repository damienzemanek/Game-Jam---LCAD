using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

[DefaultExecutionOrder(501)]
public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float crossFade = 0.2f;

    [SerializeField] bool attacking = false;
    [SerializeField] bool right;

    [SerializeField] string[] atkToLeft;
    [SerializeField] string[] atkToRight;

    private void Awake()
    {
        right = false;
    }

    private void OnEnable()
    {
        PlayerCombat.Instance.onEnemyDefenceHook.AddListener(Attack);
        PlayerCombat.Instance.onAttackHook.AddListener(Attack);

    }

    private void OnDisable()
    {
        PlayerCombat.Instance.onEnemyDefenceHook.RemoveListener(Attack);
        PlayerCombat.Instance.onAttackHook.RemoveListener(Attack);
    }


    public void Attack()
    {
        
        this.Log("attking");
        if (right == true)
            animator.CrossFade(stateName: atkToRight.Rand(), crossFade);
        else 
            animator.CrossFade(stateName: atkToLeft.Rand(), crossFade);

        right = !right;

    }

}
