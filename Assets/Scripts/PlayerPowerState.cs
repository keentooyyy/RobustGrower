using UnityEngine;
using System.Collections;

public class PlayerPowerState : MonoBehaviour
{
    public bool isGiant = false;
    public Vector3 giantScale = new Vector3(3f, 3f, 3f);
    public float pushForce = 20f;
    public float destroyDelay = 0.2f;

    private Vector3 originalScale;
    private Coroutine resetCoroutine;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void ActivateGiantMode(float duration)
    {
        // Apply giant mode scale and state
        if (!isGiant)
        {
            isGiant = true;
            transform.localScale = giantScale;
        }

        // Reset the timer if already active
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        resetCoroutine = StartCoroutine(ResetAfterTime(duration));
    }

    private IEnumerator ResetAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        transform.localScale = originalScale;
        isGiant = false;
        resetCoroutine = null;
    }

    public void HandleObstacleCollision(GameObject hitObstacle)
    {
        float explosionRadius = 5f;
        float explosionForce = pushForce;

        // Push all nearby obstacles
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Obstacles"))
            {
                Rigidbody2D rb = col.attachedRigidbody;
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = 1f;
                    rb.constraints = RigidbodyConstraints2D.None;

                    Vector2 dir = (col.transform.position - transform.position).normalized;
                    rb.AddForce(dir * explosionForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            Destroy(obj);
        }
    }

    // Optional: Draw explosion radius in Scene view
    void OnDrawGizmosSelected()
    {
        if (isGiant)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 5f);
        }
    }
}
