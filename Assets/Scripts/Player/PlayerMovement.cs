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
    public float fallMultiplier = 2.5f;  // Gravity scale multiplier when falling

    private float initialMoveSpeed;
    private Rigidbody2D rb;

    [Header("State")]
    public bool isGrounded = false;
    private bool isJumping = false;

    public AnimatorControllerHelper animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialMoveSpeed = moveSpeed;
    }

    void Update()
    {
        // Gradually increase speed over time
        if (moveSpeed < maxSpeed)
        {
            moveSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // Apply horizontal movement
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        // Apply stronger gravity when falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }



        // Handle animations
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

        // Handle jumping
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        // Handle quick fall / drop input
        if (!isGrounded && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            rb.velocity += Vector2.down * jumpForce * 0.5f * Time.deltaTime; // Tweak 0.5f multiplier as needed
        }


    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            if (!isGrounded)
            {
                isGrounded = true;
                isJumping = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGrounded = false;
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
}
