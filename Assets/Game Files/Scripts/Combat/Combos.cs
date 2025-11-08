using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Combos 
{
    MonoBehaviour host;
    [ShowInInspector] int maxAttacks { get => attacks == null ? 0 : attacks.Count; }
    [SerializeField, ReadOnly] int currentAttack;

    [SerializeReference] public List<Attack> attacks;

    public void Init(MonoBehaviour _host) => host = _host;

    public void AttackLinear() => host.StartCoroutine(C_AttackLinear());

    IEnumerator C_AttackLinear()
    {
        while(currentAttack < maxAttacks)
        {
            yield return new WaitForSeconds(attacks[currentAttack].chargeUpTime);
            attacks[currentAttack].Execute();
            currentAttack++;
        }
    }
}
