using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeapAttack : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private ParticleSystem hitParticles;
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

        Vector3 particleSpawnPos = Vector2.Lerp(EnemyController.Position, PlayerController.Position, 0.8f) + Random.insideUnitCircle;
        ParticleSystem particles = Instantiate(hitParticles, particleSpawnPos, Quaternion.identity);
        Destroy(particles, 1f);
    }
}
