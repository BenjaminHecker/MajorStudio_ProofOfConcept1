using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    public static Vector3 Position { get { return instance.transform.position; } }

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] public Transform character;
    [SerializeField] public AttackManager attackManager;
    [SerializeField] public RingManager ringManager;
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

    private Vector2 move;
    private bool grounded = true;
    private bool jump = false;

    public bool FacingRight { get { return character.localScale.x > 0; } }

    [HideInInspector] public bool freezeCharacterDirection = false;
    [HideInInspector] public bool freezeMovementY = false;

    [Header("Health")]
    [SerializeField] private float healthMax;
    [SerializeField] private float healDelay;
    [SerializeField] private float healSpeed;

    private float health;
    private float healTimer = 0f;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();

        health = healthMax;

        attackManager.Setup(this);
    }

    private void Update()
    {
        UpdateHeal();
        UpdateHealth();

        CheckGrounded();
        HandleHorizontalMovement();
        HandleJump();

        if (Input.GetKeyDown(KeyCode.X) && attackManager.SlashAttackReady)
        {
            anim.SetTrigger("Attack");
            attackManager.SlashAttack();
        }

        if (Input.GetKeyDown(KeyCode.C) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && attackManager.DashSpecialReady)
        {
            anim.SetTrigger("Dash");
            attackManager.DashSpecial();
            ringManager.RemoveRings(1);
        }

        if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.UpArrow) && attackManager.RangeSpecialReady)
        {
            anim.SetTrigger("Range Special");
            attackManager.RangeSpecial();
        }
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            rb.velocity += Vector2.up * Mathf.Sqrt(-2f * jumpHeight * gravityRise * Physics2D.gravity.y);
            jump = false;
            anim.SetBool("isAirborne", true);
            anim.SetTrigger("Jump");
        }

        if (freezeMovementY)
            rb.gravityScale = 0;
        else if (rb.velocity.y > 0)
            rb.gravityScale = gravityRise;
        else if (rb.velocity.y < 0)
            rb.gravityScale = gravityFall;

        float xVelocity = move.x * horizontalMoveSpeed;
        float yVelocity = freezeMovementY ? 0 : rb.velocity.y;

        rb.velocity = new Vector2(xVelocity, yVelocity);
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCastDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * groundCastDistance, Color.red);

        bool prevGrounded = grounded;
        grounded = hit.collider != null;

        if (grounded && !prevGrounded)
            anim.SetBool("isAirborne", false);
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

    private void UpdateHealth()
    {
        healthBar.SetHealth(Mathf.InverseLerp(0f, healthMax, health));
    }

    public static void AddRingMarker()
    {
        instance.ringManager.AddRingMarker();
    }

    public static void TakeDamage(float amount)
    {
        instance.ringManager.ResetOuterRing();

        instance.healTimer = 0f;

        instance.health = Mathf.Clamp(instance.health - amount, 0f, instance.healthMax);
        instance.UpdateHealth();
    }
    private void UpdateHeal()
    {
        healTimer += Time.deltaTime;

        if (healTimer >= healDelay)
            health = Mathf.Clamp(health + healSpeed * Time.deltaTime, 0f, healthMax);
    }
}
