using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

[Serializable]
public abstract class Attack 
{
    [SerializeField] protected float _chargeUpTime;
    [SerializeField] protected GameObject projectilePrefab;
    public float chargeUpTime { get => _chargeUpTime; set => _chargeUpTime = value; }    

    public abstract void Execute();
}

[Serializable]
public class Directional : Attack
{
    [SerializeField] Transform startPos;
    [SerializeField] Vector3 rotation;
    [SerializeField] float speed = 0.1f;
    public override void Execute()
    {
        if(projectilePrefab == null) this.Error("Directionals need projectiles");

        Quaternion baseRot = startPos.rotation;
        Vector3 newRotValues = baseRot.eulerAngles;
        newRotValues += rotation;

        GameObject projectile = GameObject.Instantiate(
            projectilePrefab,
            startPos.position,
            Quaternion.Euler(newRotValues),
            null
            );


        projectile.TryGet<Projectile>().Init(speed);

        this.Log($"Shot projectile w/ speed {speed}");
    }
}