using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private bool isDead = false;
    private Rigidbody2D rb;
    private PlayerMovement movement;
    public ScoreManager scoreManager;
    public GameOverManager gameOverManager;

    private AnimatorControllerHelper animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<AnimatorControllerHelper>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacles"))
        {
            PlayerPowerState powerState = GetComponent<PlayerPowerState>();
            if (powerState != null && powerState.isGiant)
            {
                Transform t = collision.collider.transform;
                while (t != null && t.name != "Box Vertical")
                {
                    t = t.parent;
                }

                if (t != null && t.CompareTag("Obstacles"))
                {
                    powerState.HandleObstacleCollision(t.gameObject);
                }
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

        if (movement != null)
            movement.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 2f;
        }

        if (animator != null)
        {
            animator.PlayAnimation("Death");
            StartCoroutine(WaitThenTriggerGameOver());
        }
        else
        {
            gameOverManager.TriggerGameOver();
        }

        if (scoreManager != null)
            scoreManager.StopScore();
    }

    private IEnumerator WaitThenTriggerGameOver()
    {
        // Estimate duration of "Death" animation
        yield return new WaitForSecondsRealtime(.5f); // adjust based on your actual animation

        gameOverManager.TriggerGameOver();
    }
}
