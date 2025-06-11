using UnityEngine;
using DG.Tweening;

public class PulseScale : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseScale = 1.2f;
    public float pulseDuration = 0.6f;
    public Ease pulseEase = Ease.InOutSine;
    public bool playOnAwake = true;

    private Vector3 originalScale;
    private Tween pulseTween;

    void Awake()
    {
        originalScale = transform.localScale;

        if (playOnAwake)
            StartPulse();
    }

    public void StartPulse()
    {
        StopPulse(); // clear existing

        pulseTween = transform.DOScale(originalScale * pulseScale, pulseDuration)
            .SetEase(pulseEase)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true); // Update even if timescale is 0
    }

    public void StopPulse()
    {
        if (pulseTween != null && pulseTween.IsActive())
        {
            pulseTween.Kill();
            transform.localScale = originalScale;
        }
    }

    private void OnDisable()
    {
        StopPulse();
    }
}
