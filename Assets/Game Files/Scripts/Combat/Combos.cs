using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Combos 
{
    [ShowInInspector] int maxAttacks { get => attacks == null ? 0 : attacks.Count; }
    [SerializeField, ReadOnly] int currentAttack;

    public List<Attack> attacks;

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
