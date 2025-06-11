using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private bool isDead = false;
    private Rigidbody2D rb;
    private PlayerMovement movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead && collision.collider.CompareTag("Obstacles"))
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Dead.");

        // Stop movement
        if (movement != null)
            movement.enabled = false;

        // Optional: freeze horizontal, fall downward
        if (rb != null)
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.gravityScale = 2f;
        }
         GetComponent<AnimatorControllerHelper>()?.PlayAnimation("Death");
    }
}
