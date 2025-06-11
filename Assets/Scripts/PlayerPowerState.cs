using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerPowerState : MonoBehaviour
{
    [Header("Giant Settings")]
    public bool isGiant = false;
    public Vector3 giantScale = new Vector3(3f, 3f, 3f);
    public float pushForce = 20f;

    [Header("Explosion")]
    public float explosionRadius = 5f;

    [Header("Destruction Effects")]
    public float delayBeforeBlink = 0.4f;
    public float destroyDelay = 0.6f;
    public float blinkInterval = 0.1f;

    private Vector3 originalScale;
    private Coroutine resetCoroutine;
    private PlayerMovement movement;
    private Collider2D playerCollider;

    void Start()
    {
        originalScale = transform.localScale;
        movement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider2D>();
    }

    public void ActivateGiantMode(float duration)
    {
        if (resetCoroutine != null)
            StopCoroutine(resetCoroutine);

        if (isGiant)
        {
            resetCoroutine = StartCoroutine(ResetAfterTime(duration));
            return;
        }

        transform.localScale = originalScale;

        StartCoroutine(PauseGameTemporarily(0.3f));
        if (movement != null) movement.enabled = false;

        Sequence growSeq = DOTween.Sequence().SetUpdate(true);
        float bounceTime = 0.1f;

        for (int i = 0; i < 3; i++)
        {
            growSeq.Append(transform.DOScale(giantScale * 0.6f, bounceTime).SetUpdate(true));
            growSeq.Append(transform.DOScale(giantScale * 1.1f, bounceTime).SetUpdate(true));
        }

        growSeq.Append(transform.DOScale(giantScale, 0.15f).SetUpdate(true));

        isGiant = true;

        growSeq.OnComplete(() =>
        {
            if (movement != null) movement.enabled = true;
            resetCoroutine = StartCoroutine(ResetAfterTime(duration));
        });
    }

    private IEnumerator ResetAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        yield return PauseGameTemporarily(0.3f);
        if (movement != null) movement.enabled = false;

        Sequence shrinkSeq = DOTween.Sequence().SetUpdate(true);
        float bounceTime = 0.1f;

        shrinkSeq.Append(transform.DOScale(giantScale * 0.6f, bounceTime).SetUpdate(true));
        shrinkSeq.Append(transform.DOScale(giantScale * 1.1f, bounceTime).SetUpdate(true));
        shrinkSeq.Append(transform.DOScale(originalScale * 1.2f, bounceTime).SetUpdate(true));
        shrinkSeq.Append(transform.DOScale(originalScale, 0.15f).SetUpdate(true));

        shrinkSeq.OnComplete(() =>
        {
            isGiant = false;
            resetCoroutine = null;
            if (movement != null) movement.enabled = true;
        });
    }

    private IEnumerator PauseGameTemporarily(float realSeconds)
    {
        Time.timeScale = 0f;
        float timer = 0f;
        while (timer < realSeconds)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
    }

    public void HandleObstacleCollision(GameObject hitObstacle)
    {
        if (!hitObstacle.CompareTag("Obstacles")) return;

        // Disable parent collider so it doesn't block the player
        Collider2D parentCollider = hitObstacle.GetComponent<Collider2D>();
        if (parentCollider != null)
        {
            parentCollider.enabled = false;
        }

        Transform parent = hitObstacle.transform.parent;
        if (parent == null) return;

        Rigidbody2D[] boxesToPush = parent.GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D rb in boxesToPush)
        {
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1f;
                rb.constraints = RigidbodyConstraints2D.None;

                Vector2 dir = (rb.transform.position - transform.position).normalized;
                rb.AddForce(dir * pushForce, ForceMode2D.Impulse);

                StartCoroutine(DelayedBlinkAndDestroy(rb.gameObject, delayBeforeBlink, destroyDelay));
            }
        }
    }

    private IEnumerator DelayedBlinkAndDestroy(GameObject obj, float delayBeforeBlink, float blinkDuration)
    {
        yield return new WaitForSeconds(delayBeforeBlink);

        // Disable colliders to prevent re-collision
        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        // Blink effect
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float timer = 0f;
            while (timer < blinkDuration)
            {
                sr.enabled = false;
                yield return new WaitForSeconds(blinkInterval);
                sr.enabled = true;
                yield return new WaitForSeconds(blinkInterval);
                timer += blinkInterval * 2;
            }
        }

        Destroy(obj);
    }
}
