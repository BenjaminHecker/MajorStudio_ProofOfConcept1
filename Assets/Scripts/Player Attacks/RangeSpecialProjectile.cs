using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSpecialProjectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitParticles;

    private Rigidbody2D rb;

    [Space]
    [SerializeField] private Vector2 windUpOffset;
    [SerializeField] private float windUpGroupFactor = 2f;
    [SerializeField] private AnimationCurve windUpMoveCurve;

    [Space]
    [SerializeField] private float launchWait;
    [SerializeField] private float launchSpeed;

    private float launchTime;
    private Vector3 windUpTarget;
    private Vector3 launchTarget;

    private bool windingUp = false;

    private float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(PlayerController player, Vector3 enemyPos, float launchTime, float damage)
    {
        Vector3 offset = windUpOffset;

        if (player.transform.position.x > enemyPos.x) offset.x *= -1;

        windUpTarget = player.transform.position + offset + (transform.position - player.transform.position) * windUpGroupFactor;

        launchTarget = enemyPos;

        this.launchTime = launchTime;
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (windingUp) return;

        if (collision.CompareTag("Enemy"))
        {
            HitEnemy();
            Destroy(gameObject);
        }

        if (collision.CompareTag("Bounds"))
        {
            Destroy(gameObject);
        }
    }

    private void HitEnemy()
    {
        PlayerController.AddRingMarker();
        EnemyController.TakeDamage(damage);

        Vector3 particleSpawnPos = Vector2.Lerp(transform.position, EnemyController.Position, 0.5f) + Random.insideUnitCircle;
        ParticleSystem particles = Instantiate(hitParticles, particleSpawnPos, Quaternion.identity);
        Destroy(particles, 1f);
    }

    public void Trigger()
    {
        StartCoroutine(TriggerRoutine());
    }

    private IEnumerator TriggerRoutine()
    {
        float windUpTimer = 0f;
        float windUpDuration = launchTime - Time.time;

        windingUp = true;

        Vector3 startPos = transform.position;
        Vector3 startRot = transform.up;

        while (windUpTimer < windUpDuration)
        {
            float curvedRatio = windUpMoveCurve.Evaluate(windUpTimer / windUpDuration);

            transform.position = Vector3.Lerp(startPos, windUpTarget, curvedRatio);
            transform.up = Vector3.Lerp(startRot, launchTarget - windUpTarget, curvedRatio);

            windUpTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(launchWait);

        windingUp = false;

        rb.velocity = (launchTarget - windUpTarget).normalized * launchSpeed;
    }
}
