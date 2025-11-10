using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.VFX;

[DefaultExecutionOrder(501)]
public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float crossFade = 0.2f;

    [SerializeField] bool attacking = false;
    [SerializeField] bool right;

    [SerializeField] string[] atkToLeft;
    [SerializeField] string[] atkToRight;

    [SerializeField] VisualEffect rightToLeft;
    [SerializeField] VisualEffect leftToRight;

    private void Awake()
    {
        right = false;

        rightToLeft.gameObject.SetActive(false);
        leftToRight.gameObject.SetActive(false);    
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
        {
            animator.CrossFade(stateName: atkToRight.Rand(), crossFade);
            leftToRight.gameObject.SetActive(true);
            leftToRight.Play();
        }
        else
        {
            animator.CrossFade(stateName: atkToLeft.Rand(), crossFade);
            rightToLeft.gameObject.SetActive(true);
            rightToLeft.Play();
        }
        right = !right;

    }

}
