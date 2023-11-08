using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSpecial : MonoBehaviour
{
    private PlayerController player;

    [Header("References")]
    [SerializeField] private RangeSpecialProjectile projectilePrefab;

    [Header("Config")]
    [SerializeField] private float spawnTick;
    [SerializeField] private float launchDelay = 1f;

    [Space]
    [SerializeField] private float damage;

    public void Setup(PlayerController player)
    {
        this.player = player;
    }

    public void Trigger()
    {
        StartCoroutine(TriggerRoutine());
    }

    private IEnumerator TriggerRoutine()
    {
        float ringCounter = 0;

        if (ringCounter < 2 && player.ringManager.FirstRingComplete)
        {
            ringCounter++;

            float launchTime = Time.time + launchDelay;

            while (player.ringManager.firstRingMarkers.Count > 0)
            {
                RingMarker marker = player.ringManager.firstRingMarkers[0];

                RangeSpecialProjectile projectile = Instantiate(projectilePrefab, marker.transform.position, marker.transform.rotation);
                projectile.transform.localScale = marker.transform.lossyScale;
                projectile.Setup(player, EnemyController.Position, launchTime, damage);
                projectile.Trigger();

                player.ringManager.firstRingMarkers.RemoveAt(0);
                Destroy(marker.gameObject);

                yield return new WaitForSeconds(spawnTick);
            }
        }
        if (ringCounter < 2 && player.ringManager.SecondRingComplete)
        {
            ringCounter++;

            float launchTime = Time.time + launchDelay;

            while (player.ringManager.secondRingMarkers.Count > 0)
            {
                RingMarker marker = player.ringManager.secondRingMarkers[0];

                RangeSpecialProjectile projectile = Instantiate(projectilePrefab, marker.transform.position, marker.transform.rotation);
                projectile.transform.localScale = marker.transform.lossyScale;
                projectile.Setup(player, EnemyController.Position, launchTime, damage);
                projectile.Trigger();

                player.ringManager.secondRingMarkers.RemoveAt(0);
                Destroy(marker.gameObject);

                yield return new WaitForSeconds(spawnTick);
            }
        }
        if (ringCounter < 2 && player.ringManager.ThirdRingComplete)
        {
            ringCounter++;

            float launchTime = Time.time + launchDelay;

            while (player.ringManager.thirdRingMarkers.Count > 0)
            {
                RingMarker marker = player.ringManager.thirdRingMarkers[0];

                RangeSpecialProjectile projectile = Instantiate(projectilePrefab, marker.transform.position, marker.transform.rotation);
                projectile.transform.localScale = marker.transform.lossyScale;
                projectile.Setup(player, EnemyController.Position, launchTime, damage);
                projectile.Trigger();

                player.ringManager.thirdRingMarkers.RemoveAt(0);
                Destroy(marker.gameObject);

                yield return new WaitForSeconds(spawnTick);
            }
        }
    }
}
