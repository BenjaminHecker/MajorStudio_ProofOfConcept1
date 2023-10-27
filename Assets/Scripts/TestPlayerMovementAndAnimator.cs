using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovementAndAnimator : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    public Animator animator;
    private bool isJumping = false;
    private bool IsInMove = false;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
      

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0);
        transform.position += movement * Time.deltaTime * moveSpeed;

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
           
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        

        if (horizontalInput > 0.01f)
        {
            transform.localScale = originalScale;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isJumping)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            animator.SetTrigger("Jump");
            animator.SetBool("IsAirborne", true);
           
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Attack");
            
        }

        animator.SetBool("IsInMove", IsInMove);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
           
            isJumping = false;
            animator.SetBool("IsAirborne", false);
           
        }
    }
}
