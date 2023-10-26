using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private RingManager ringManager;

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
    private bool grounded = false;
    private bool jump = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCastDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * groundCastDistance, Color.red);

        grounded = hit.collider != null;

        if (Input.GetKeyDown(KeyCode.Z) && grounded)
            jump = true;

        if (Input.GetKeyDown(KeyCode.X))
            ringManager.AddRingMarker();

        if (Input.GetKeyDown(KeyCode.C))
            ringManager.ResetOuterRing();
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            rb.velocity += Vector2.up * Mathf.Sqrt(-2f * jumpHeight * gravityRise * Physics2D.gravity.y);
            jump = false;
        }

        if (rb.velocity.y > 0)
            rb.gravityScale = gravityRise;
        else if (rb.velocity.y < 0)
            rb.gravityScale = gravityFall;

        rb.velocity = new Vector2(move.x * horizontalMoveSpeed, rb.velocity.y);
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
}
