using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private bool isDead = false;
    private Rigidbody2D rb;
    private PlayerMovement movement;
    public ScoreManager scoreManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacles"))
        {
            PlayerPowerState powerState = GetComponent<PlayerPowerState>();
            if (powerState != null && powerState.isGiant)
            {
                powerState.HandleObstacleCollision(collision.collider.gameObject);
            }
            else if (!isDead)
            {
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true;
        //Debug.Log("Dead.");

        if (movement != null)
            movement.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 2f;
        }

        GetComponent<AnimatorControllerHelper>()?.PlayAnimation("Death");

        if (scoreManager != null)
            scoreManager.StopScore();
    }
}
