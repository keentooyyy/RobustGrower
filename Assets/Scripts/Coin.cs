using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCoins playerCoins = other.GetComponent<PlayerCoins>();
            if (playerCoins != null)
            {
                playerCoins.AddCoins(value);
            }

            Destroy(gameObject);
        }
    }
}
