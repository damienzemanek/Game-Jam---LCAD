using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class EnemyHealth : Health
{
    protected override void AdditionalAwakeImplements()
    {
        slider = PlayerCombat.Instance.enemyHealthSlider;
        this.Log($"Grabbing slider {slider.name}");
    }

    public override void Die()
    {
        PlayerCombat.Instance.KillEnemy();
    }
}
