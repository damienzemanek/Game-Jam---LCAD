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
    [SerializeField] bool left, right;

    [SerializeField] string[] atkToLeft;
    [SerializeField] string[] atkToRight;

    private void Awake()
    {
        right = true;
        left = false;
    }

    private void OnEnable()
    {
        PlayerCombat.Instance.onEnemyDefenceHook.AddListener(Attack);
    }

    private void OnDisable()
    {
        PlayerCombat.Instance.onEnemyDefenceHook.RemoveListener(Attack);
    }


    public void Attack()
    {
        this.Log("attking");
        attacking = true;
        if (right)
            animator.PlayWithHook(atkToLeft.Rand(), this, IsNowLeft);
        else
            animator.PlayWithHook(atkToRight.Rand(), this, IsNowRight);

    }


    void IsNowLeft()
    {
        attacking = false;
        right = false;
        left = true;

    }

    void IsNowRight()
    {
        attacking = false;
        right = true;
        left = false;
    }

}
