using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour
{
    PlayerCombat combat;
    [SerializeField] float speed;
    [SerializeField] float _dmg = 1f;
    [SerializeField] int projectileHealth = 1;
    [SerializeField] Vector3 dirOfTravel;

    public float dmg { get => _dmg; set => _dmg = value; }
    

    public void Init()
    {
       
    }

    private void Awake()
    {
        if (dmg <= 0) this.Error("projectile damage is 0, set to a number higher than 0");
        combat = PlayerCombat.Instance;
    }

    private void FixedUpdate()
    {
        //ransform.position = transform.position + dirOfTravel * speed;
        transform.Translate(dirOfTravel * speed);
    }

    public void ClickedOn()
    {
        projectileHealth--;

        this.Log("Projectile clicked on");

        if (projectileHealth <= 0) ProjDie();
    }

    void ProjDie()
    {
        speed = 0;
        transform.Children().ToList().ForEach(c => c.SetActive(false));
        Instantiate(combat.destroyProjEffect,
            transform.position,
            Quaternion.identity,
            transform
            );

        this.DelayedCall(() => Destroy(gameObject), 2f);
    }
}
