using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeapAttack : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private float damage;

    private void Start()
    {
        hurtbox.onHit += OnHit;
    }

    private void OnDestroy()
    {
        hurtbox.onHit -= OnHit;
    }

    public void StartAttack()
    {
        hurtbox.active = true;
    }

    public void EndAttack()
    {
        hurtbox.active = false;
    }

    private void OnHit()
    {
        hurtbox.active = false;
        PlayerController.TakeDamage(damage);
    }
}
