using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    private EnemyController enemy;

    [Header("Swipe")]
    [SerializeField] private float swipeDelay;
    [SerializeField] private float swipeDuration;

    [Header("Leap")]
    [SerializeField] private float leapDelay;
    [SerializeField] private float leapDuration;

    [Header("Roar")]
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
        enemy.anim.SetTrigger("Swipe");

        yield return new WaitForSeconds(swipeDelay);

        // trigger SwipeAttack

        yield return new WaitForSeconds(swipeDuration);

        enemy.Idle();
    }

    private IEnumerator LeapRoutine()
    {
        enemy.anim.SetTrigger("Leap");

        yield return new WaitForSeconds(leapDelay);

        // trigger LeapAttack

        yield return new WaitForSeconds(leapDuration);

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
