using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    public bool isGrounded = false;
    private bool isJumping = false;

    public AnimatorControllerHelper animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        // Handle animation state
        if (isGrounded)
        {
            if (!isJumping) animator.PlayAnimation("Running");
        }
        else
        {
            if (!isJumping)
            {
                animator.PlayAnimation("Jump");
                isJumping = true;
            }
        }

        // Handle jump input
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
}
