using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Combos 
{
    MonoBehaviour host;
    [SerializeField, ReadOnly] int currentAttack;

    [SerializeReference] public List<Attack> attacks;

    public void Init(MonoBehaviour _host) => host = _host;

    [Button]
    public void AttackLinear(Action postHook = null) => host.StartCoroutine(C_AttackLinear(postHook));

    IEnumerator C_AttackLinear(Action postHook = null)
    {
        float last = 0f;

        while(currentAttack < attacks.Count)
        {
            float wait = attacks[currentAttack].chargeUpTime - last;

            if(wait > 0) yield return new WaitForSeconds(wait);
            this.Log($"attacking w/ {currentAttack}");
            attacks[currentAttack].Execute();

            last = attacks[currentAttack].chargeUpTime;
            currentAttack++;
        }

        postHook?.Invoke();
    }
}
