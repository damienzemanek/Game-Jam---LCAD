using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    PlayerCombat combat;
    [SerializeField] float speed;

    public void Init(float _speed)
    {
        _speed = speed;
    }

    private void Awake()
    {
        combat = PlayerCombat.Instance;
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.forward * speed);
    }
}
