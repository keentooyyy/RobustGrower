using UnityEngine;
using System.Collections;

public class GiantObstacleDestroyer : MonoBehaviour
{
    [Header("Giant Interaction")]
    public float pushForce = 20f;

    [Header("Visual Destruction")]
    public float delayBeforeBlink = 0.4f;
    public float destroyDelay = 0.6f;
    public float blinkInterval = 0.1f;

    private bool isDestroyed = false;

    public void TriggerDestruction(Vector3 playerPosition)
    {
        if (isDestroyed) return;
        isDestroyed = true;

        // Push rigidbodies first
        Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D>();
        foreach (var rb in rbs)
        {
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.simulated = true;
                rb.gravityScale = 1f;
                rb.constraints = RigidbodyConstraints2D.None;

                Vector2 dir = (rb.transform.position - playerPosition).normalized;
                rb.AddForce(dir * pushForce, ForceMode2D.Impulse);
            }
        }

        // Disable all colliders immediately to prevent blocking
        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (var col in cols)
            col.enabled = false;

        StartCoroutine(BlinkAndDestroy());
    }


    private IEnumerator BlinkAndDestroy()
    {
        yield return new WaitForSeconds(delayBeforeBlink);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float timer = 0f;
            while (timer < destroyDelay)
            {
                sr.enabled = false;
                yield return new WaitForSeconds(blinkInterval);
                sr.enabled = true;
                yield return new WaitForSeconds(blinkInterval);
                timer += blinkInterval * 2;
            }
        }

        Destroy(gameObject);
    }
}
