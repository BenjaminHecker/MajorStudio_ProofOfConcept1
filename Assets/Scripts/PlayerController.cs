using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform character;
    [SerializeField] private AttackManager attackManager;
    [SerializeField] private RingManager ringManager;
    [SerializeField] private HealthBar healthBar;

    private Rigidbody2D rb;

    [Header("Config")]
    [SerializeField] private float horizontalMoveSpeed = 1f;

    [Space]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravityRise = 5f;
    [SerializeField] private float gravityFall = 5f;
    [SerializeField] private float groundCastDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Space]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;

    private Vector2 move;
    private bool grounded = true;
    private bool jump = false;
    private bool dashing = false;

    public static bool freezeCharacterDirection = false;

    [Header("Health")]
    [SerializeField] private float healthMax;
    [SerializeField] private float healAmount;

    private float health;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();

        health = healthMax;
    }

    private void Update()
    {
        CheckGrounded();
        HandleHorizontalMovement();
        HandleJump();

        if (Input.GetKeyDown(KeyCode.X) && attackManager.SlashAttackReady)
        {
            anim.SetTrigger("Attack");
            attackManager.SlashAttack();

            freezeCharacterDirection = true;
        }
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            rb.velocity += Vector2.up * Mathf.Sqrt(-2f * jumpHeight * gravityRise * Physics2D.gravity.y);
            jump = false;
            anim.SetTrigger("Jump");
        }

        if (rb.velocity.y > 0)
            rb.gravityScale = gravityRise;
        else if (rb.velocity.y < 0)
            rb.gravityScale = gravityFall;

        float xVelocity = move.x * horizontalMoveSpeed;

        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCastDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * groundCastDistance, Color.red);

        bool prevGrounded = grounded;
        grounded = hit.collider != null;

        if (grounded && !prevGrounded)
            anim.SetTrigger("Land");
    }

    private void HandleHorizontalMovement()
    {
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (move.x > 0.1f)
        {
            if (!freezeCharacterDirection)
            {
                Vector3 scale = character.localScale;
                scale.x = Mathf.Abs(scale.x);
                character.localScale = scale;
            }

            anim.SetBool("isMoving", true);
        }
        else if (move.x < -0.1f)
        {
            if (!freezeCharacterDirection)
            {
                Vector3 scale = character.localScale;
                scale.x = -Mathf.Abs(scale.x);
                character.localScale = scale;
            }

            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Z) && grounded && !jump)
            jump = true;
    }

    public static Vector2 SnapAngle(Vector2 vector, int increments = 8)
    {
        float angle = Mathf.Atan2(vector.y, vector.x);
        float direction = ((angle / Mathf.PI) + 1) * 0.5f;
        float snappedDirection = Mathf.Round(direction * increments) / increments;
        snappedDirection = ((snappedDirection * 2) - 1) * Mathf.PI;
        Vector2 snappedVector = new Vector2(Mathf.Cos(snappedDirection), Mathf.Sin(snappedDirection));
        return vector.magnitude * snappedVector;
    }

    public static void HitEnemy()
    {
        instance.ringManager.AddRingMarker();

        instance.health = Mathf.Clamp(instance.health + instance.healAmount, 0f, instance.healthMax);
        instance.UpdateHealth();
    }

    public void Damage(Vector3 source, float amount)
    {
        ringManager.ResetOuterRing();

        health = Mathf.Clamp(health - amount, 0f, healthMax);
        UpdateHealth();

        rb.velocity += (transform.position - source) * Vector2.right * 5f;
    }

    private void UpdateHealth()
    {
        healthBar.SetHealth(Mathf.InverseLerp(0f, healthMax, health));
    }
}
