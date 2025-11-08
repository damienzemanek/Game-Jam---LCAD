using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    PlayerCombat combat;
    [SerializeField, ReadOnly] float speed;

    public void Init(float _speed)
    {
        speed = _speed;
    }

    private void Awake()
    {
        combat = PlayerCombat.Instance;
    }

    private void FixedUpdate()
    {
        transform.Translate(-transform.forward * speed);
    }
}
