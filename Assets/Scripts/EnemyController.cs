using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private static EnemyController instance;

    public static Vector3 Position { get { return instance.transform.position; } }

    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Animator anim;
    [SerializeField] public Transform character;
    [SerializeField] public EnemyAttackManager attackManager;


    [Header("Config")]
    [SerializeField] private float idleTime;

    [Space]
    [SerializeField] private float walkDelay;
    [SerializeField] private float walkDuration;
    [SerializeField] private float walkMoveSpeed;

    [Space]
    [SerializeField] [Range(0f, 1f)] private float walkChance;
    [SerializeField] [Range(0f, 1f)] private float swipeChance;
    [SerializeField] [Range(0f, 1f)] private float leapChance;

    public bool FacingRight { get { return character.localScale.x > 0; } }


    [Header("Health")]
    [SerializeField] private float healthMax;
    [SerializeField] private float health;

    private void Awake()
    {
        instance = this;

        health = healthMax;

        attackManager.Setup(this);
    }

    private void Start()
    {
        Idle();
    }

    public void Idle() { StartCoroutine(IdleRoutine()); }
    private void Walk() { StartCoroutine(WalkRoutine()); }
    private void Attack()
    {
        float rng = Random.value;

        if (rng <= swipeChance)
            attackManager.Swipe();
        else if (rng <= swipeChance + leapChance)
            attackManager.Leap();
        else
            attackManager.Roar();
    }

    private IEnumerator IdleRoutine()
    {
        UpdateFaceDirection();

        anim.SetBool("Walking", false);
        anim.SetBool("Running", false);

        yield return new WaitForSeconds(idleTime);

        UpdateFaceDirection();

        if (Random.value <= walkChance) Walk();
        else Attack();
    }

    private IEnumerator WalkRoutine()
    {
        anim.SetBool("Walking", true);

        yield return new WaitForSeconds(walkDelay);

        for (float walkTimer = 0f; walkTimer < walkDuration; walkTimer += Time.deltaTime)
        {
            transform.position += (FacingRight ? Vector3.right : Vector3.left) * walkMoveSpeed * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        anim.SetBool("Walking", false);

        Idle();
    }

    public void UpdateFaceDirection()
    {
        Vector3 characterScale = character.localScale;

        if (PlayerController.Position.x > transform.position.x)
            characterScale.x = Mathf.Abs(characterScale.x);
        else
            characterScale.x = -Mathf.Abs(characterScale.x);

        character.localScale = characterScale;
    }

    public static void TakeDamage(float amount)
    {
        instance.health = Mathf.Clamp(instance.health - amount, 0f, instance.healthMax);

        instance.anim.SetTrigger("Hurt");
    }
}
