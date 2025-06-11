using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxSpeed = 15f;
    public float speedIncreaseRate = 0.3f;
    public float jumpForce = 12f;

    [Header("Gravity Settings")]
    public float fallMultiplier = 2.5f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private float initialMoveSpeed;
    private Rigidbody2D rb;

    [Header("State")]
    public bool isGrounded = false;
    private bool isJumping = false;

    public AnimatorControllerHelper animator;

    [HideInInspector] public bool isControlPaused = false;

    private PlayerPowerState powerState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialMoveSpeed = moveSpeed;
        powerState = GetComponent<PlayerPowerState>();
    }

    void Update()
    {
        if (isControlPaused) return;

        // ✅ Always perform ground check, even in Giant Mode
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Gradually increase speed
        if (moveSpeed < maxSpeed)
            moveSpeed += speedIncreaseRate * Time.deltaTime;

        // Apply horizontal movement
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        // Stronger gravity while falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // ✅ Allow animation only if NOT in Giant Mode
        bool allowAnimation = !(powerState != null && powerState.isGiant);
        if (allowAnimation)
        {
            if (isGrounded)
            {
                if (!isJumping)
                    animator.PlayAnimation("Running");
            }
            else
            {
                if (!isJumping)
                {
                    animator.PlayAnimation("Jump");
                    isJumping = true;
                }
            }
        }

        // ✅ Jumping (allowed even in Giant Mode)
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Quick fall
        if (!isGrounded && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            rb.velocity += Vector2.down * jumpForce * 0.5f * Time.deltaTime;
        }

        // Reset jump state
        if (isGrounded)
        {
            isJumping = false;
        }
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = initialMoveSpeed;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
