using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_CharacterController : MonoBehaviour
{
    private Animator _animator;
    public float moveSpeed;
    private Vector3 _originalScale;
    private bool _isJumping;
    private Rigidbody2D _rb;
    public float _jumpForce;
    private bool _airbone;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _originalScale = transform.localScale;
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Z) && !_isJumping)
        {
            Jump();
        }
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0);
        transform.position += movement * Time.deltaTime * moveSpeed;

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {

            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }


        if (horizontalInput > 0.01f)
        {
            transform.localScale = _originalScale;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-_originalScale.x, _originalScale.y, _originalScale.z);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _animator.SetTrigger("Attack");
        }
    }

    void Jump()
    {
        _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        _isJumping = true;
        _animator.SetTrigger("Jump");
        _airbone = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (_airbone)
            {
                _animator.SetTrigger("Land");
                _isJumping = false;

            }
        }
    }
}
