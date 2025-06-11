using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerPowerState : MonoBehaviour
{
    [Header("Giant Mode Settings")]
    public bool isGiant = false;
    public Vector3 giantScale = new Vector3(3f, 3f, 3f);
    public float giantDuration = 5f;

    [Header("Audio")]
    public AudioClip powerUpSound;
    public AudioClip powerDownSound;

    private Vector3 originalScale;
    private Coroutine resetCoroutine;
    private PlayerMovement movement;
    private AudioSource audioSource;

    void Start()
    {
        originalScale = transform.localScale;
        movement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
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

        StartCoroutine(PauseGameTemporarily(0.3f));
        if (movement != null) movement.isControlPaused = true;

        // 🔊 Play power-up sound
        if (powerUpSound != null && audioSource != null)
            audioSource.PlayOneShot(powerUpSound);

        transform.localScale = originalScale;

        Sequence growSeq = DOTween.Sequence().SetUpdate(true);
        float bounce = 0.1f;
        for (int i = 0; i < 3; i++)
        {
            growSeq.Append(transform.DOScale(giantScale * 0.6f, bounce).SetUpdate(true));
            growSeq.Append(transform.DOScale(giantScale * 1.1f, bounce).SetUpdate(true));
        }
        growSeq.Append(transform.DOScale(giantScale, 0.15f).SetUpdate(true));

        isGiant = true;
        growSeq.OnComplete(() =>
        {
            if (movement != null) movement.isControlPaused = false;
            resetCoroutine = StartCoroutine(ResetAfterTime(duration));
        });
    }

    private IEnumerator ResetAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        yield return PauseGameTemporarily(0.3f);
        if (movement != null) movement.isControlPaused = true;

        // 🔊 Play power-down sound
        if (powerDownSound != null && audioSource != null)
            audioSource.PlayOneShot(powerDownSound);

        Sequence shrinkSeq = DOTween.Sequence().SetUpdate(true);
        float bounce = 0.1f;
        shrinkSeq.Append(transform.DOScale(giantScale * 0.6f, bounce).SetUpdate(true));
        shrinkSeq.Append(transform.DOScale(giantScale * 1.1f, bounce).SetUpdate(true));
        shrinkSeq.Append(transform.DOScale(originalScale * 1.2f, bounce).SetUpdate(true));
        shrinkSeq.Append(transform.DOScale(originalScale, 0.15f).SetUpdate(true));

        shrinkSeq.OnComplete(() =>
        {
            isGiant = false;
            resetCoroutine = null;
            if (movement != null) movement.isControlPaused = false;
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

    public void HandleObstacleCollision(GameObject boxVertical)
    {
        GiantObstacleDestroyer destroyer = boxVertical.GetComponent<GiantObstacleDestroyer>();
        if (destroyer != null)
        {
            destroyer.TriggerDestruction(transform.position);
        }
    }
}
