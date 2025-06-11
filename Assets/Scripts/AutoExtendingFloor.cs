using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AutoExtendingFloor : MonoBehaviour
{
    public Transform cameraTransform;
    public float extendAhead = 20f;
    public float shrinkBehind = 10f;

    private float originalWidthWorld;
    private Vector3 originalPosition;
    private Vector3 originalScale;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        originalScale = transform.localScale;
        originalPosition = transform.position;

        // Calculate width in world units
        originalWidthWorld = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float camX = cameraTransform.position.x;

        float leftEdge = camX - shrinkBehind;
        float rightEdge = camX + extendAhead;

        float newWidth = rightEdge - leftEdge;

        float scaleFactor = newWidth / originalWidthWorld;

        // Scale floor
        transform.localScale = new Vector3(scaleFactor * originalScale.x, originalScale.y, originalScale.z);

        // Move floor so its left edge aligns with leftEdge
        transform.position = new Vector3(
            leftEdge + (newWidth / 2f),
            originalPosition.y,
            originalPosition.z
        );
    }
}
