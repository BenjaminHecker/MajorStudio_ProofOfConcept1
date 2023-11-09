using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    private EnemyController enemy;

    [Header("Swipe")]
    [SerializeField] private EnemySwipeAttack swipeAttack;

    [Space]
    [SerializeField] private float runStartDelay;
    [SerializeField] private float runMoveSpeed;
    [SerializeField] private float runEndDelay;

    [Space]
    [SerializeField] private float swipeDelay;
    [SerializeField] private float firstSwipeDuration;
    [SerializeField] private float secondSwipeDuration;
    [SerializeField] private float swipeDistance;

    [Header("Leap")]
    [SerializeField] private EnemyLeapAttack leapAttack;

    [Space]
    [SerializeField] private float leapDelay;
    [SerializeField] private float leapDuration;
    [SerializeField] private float leapSpeed;
    [SerializeField] private float leapHorizontalMax;

    [Header("Roar")]
    [SerializeField] private EnemyRoarAttack roarAttack;

    [Space]
    [SerializeField] private float roarDelay;
    [SerializeField] private float roarDuration;

    public void Setup(EnemyController enemyController)
    {
        enemy = enemyController;
    }

    public void Swipe() { StartCoroutine(SwipeRoutine()); }

    public void Leap() { StartCoroutine(LeapRoutine()); }

    public void Roar() { StartCoroutine(RoarRoutine()); }

    private IEnumerator SwipeRoutine()
    {
        enemy.UpdateFaceDirection();
        float distanceToPlayer = Mathf.Abs(PlayerController.Position.x - EnemyController.Position.x);

        if (distanceToPlayer > swipeDistance)
        {
            enemy.anim.SetBool("Running", true);

            yield return new WaitForSeconds(runStartDelay);

            while (distanceToPlayer > swipeDistance)
            {
                enemy.transform.position += (enemy.FacingRight ? Vector3.right : Vector3.left) * runMoveSpeed * Time.deltaTime;

                distanceToPlayer = Mathf.Abs(PlayerController.Position.x - EnemyController.Position.x);
                yield return new WaitForEndOfFrame();
            }

            enemy.anim.SetBool("Running", false);

            yield return new WaitForSeconds(runEndDelay);
        }

        enemy.anim.SetTrigger("Swipe");

        SoundManager.PlayMisc("Enemy Swipe Attack");

        yield return new WaitForSeconds(swipeDelay);

        swipeAttack.StartAttack();
        yield return new WaitForSeconds(firstSwipeDuration);
        swipeAttack.StartAttack();
        yield return new WaitForSeconds(secondSwipeDuration);
        swipeAttack.EndAttack();

        enemy.Idle();
    }

    private IEnumerator LeapRoutine()
    {
        enemy.UpdateFaceDirection();
        float initialX = EnemyController.Position.x;
        float targetX = PlayerController.Position.x;

        if (Mathf.Abs(targetX - initialX) > leapHorizontalMax)
            targetX = initialX + (enemy.FacingRight ? 1 : -1) * leapHorizontalMax;

        enemy.anim.SetTrigger("Leap");

        SoundManager.PlayMisc("Enemy Leap");

        yield return new WaitForSeconds(leapDelay);

        enemy.rb.velocity += Vector2.up * leapSpeed;

        bool activatedHurtbox = false;

        for (float leapTimer = 0f; leapTimer < leapDuration; leapTimer += Time.deltaTime)
        {
            Vector3 pos = EnemyController.Position;
            pos.x = Mathf.Lerp(initialX, targetX, leapTimer / leapDuration);
            enemy.transform.position = pos;

            if (leapTimer / leapDuration >= 0.5f && !activatedHurtbox)
            {
                leapAttack.StartAttack();
                activatedHurtbox = true;
            }

            yield return new WaitForEndOfFrame();
        }

        leapAttack.EndAttack();

        enemy.Idle();
    }

    private IEnumerator RoarRoutine()
    {
        enemy.anim.SetTrigger("Roar");

        yield return new WaitForSeconds(roarDelay);

        // trigger RoarAttack

        yield return new WaitForSeconds(roarDuration);

        enemy.Idle();
    }
}
