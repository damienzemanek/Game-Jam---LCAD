using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    PlayerCombat playerCombat;

    private void OnEnable()
    {
        playerCombat = this.TryGet<PlayerCombat>();
        playerCombat.onTakeDmgHook?.AddListener(TakeDamage);
    }

    public override void Die()
    {

    }

    private void Awake()
    {
        if(onDeathHook == null) onDeathHook = new UnityEventPlus();
    }


}
