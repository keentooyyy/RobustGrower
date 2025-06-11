using UnityEngine;

public class ChromeDinoCamera : MonoBehaviour
{
    public Transform target;   // Player
    public float yLock = 3.7f; // Fixed Y position
    private Vector3 offset;    // Initial offset from player

    void Start()
    {
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = new Vector3(desiredPosition.x, yLock, desiredPosition.z);
    }

    void OnDrawGizmos()
    {
        if (target == null) return;

        Vector3 desiredPosition = Application.isPlaying
            ? target.position + offset
            : transform.position;

        Vector3 lockedPosition = new Vector3(desiredPosition.x, yLock, desiredPosition.z);

        // Line from player to camera
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(target.position, lockedPosition);

        // Locked camera position
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(lockedPosition, 0.2f);

        // Approx camera view bounds
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(lockedPosition, new Vector3(2, 2, 0));
    }
}
