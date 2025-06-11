using UnityEngine;

public class GiantPowerup : MonoBehaviour
{
    public float duration = 5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPowerState powerState = other.GetComponent<PlayerPowerState>();
            if (powerState != null)
            {
                powerState.ActivateGiantMode(duration);
            }

            Destroy(gameObject); // Destroy the power-up after pickup
        }
    }
}
