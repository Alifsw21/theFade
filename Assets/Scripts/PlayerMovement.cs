using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpSpeed = 8f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.12f;

    private float horizontalInput;
    private bool isRunning;
    private bool jumpRequested;
    private bool isGrounded;
    private bool jumpInProgress;
    private bool hasLeftGround;
    private bool isDead;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isDead)
            return;

        horizontalInput = 0f;
        isRunning = false;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            horizontalInput -= 1f;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            horizontalInput += 1f;
        }

        if (Keyboard.current.shiftKey.isPressed &&
            horizontalInput != 0f)
        {
            isRunning = true;
        }

        if (spriteRenderer != null)
        {
            if (horizontalInput > 0)
            {
                spriteRenderer.flipX = false;
            } 
            else if (horizontalInput < 0)
            {
                spriteRenderer.flipX = true;
            }
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (animator != null)
                animator.SetTrigger("Attack");
        }

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        bool touchingGround = groundCheck != null &&
            Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            ) != null;

        if (jumpInProgress)
        {
            if (!touchingGround)
                hasLeftGround = true;

            if (hasLeftGround && touchingGround && body.linearVelocity.y <= 0f)
            {
                jumpInProgress = false;
            }
        }

        isGrounded = touchingGround && !jumpInProgress;

        if (!isDead)
        {
            float currentSpeed = isRunning ? runSpeed : moveSpeed;

            body.linearVelocity = new Vector2(
                horizontalInput * currentSpeed,
                body.linearVelocity.y
            );

            if (jumpRequested && isGrounded)
            {
                body.linearVelocity = new Vector2(
                    body.linearVelocity.x,
                    jumpSpeed
                );

                jumpInProgress = true;
                hasLeftGround = false;
                isGrounded = false;

                if (animator != null)
                    animator.SetTrigger("Jump");
            }
        }
        else
        {
            body.linearVelocity = new Vector2(
                0f,
                body.linearVelocity.y
            );
        }

        jumpRequested = false;

        if (animator != null)
        {
            animator.SetFloat(
                "Speed",
                Mathf.Abs(body.linearVelocity.x)
            );

            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        horizontalInput = 0f;
        jumpRequested = false;

        if (animator != null)
        {
            animator.ResetTrigger("Jump");
            animator.ResetTrigger("Attack");
            animator.SetBool("IsDead", true);
        }
    }
}