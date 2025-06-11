using UnityEngine;

public class AutoDestroyChunk : MonoBehaviour
{
    [Tooltip("How far behind the player (in X units) before this chunk is destroyed")]
    public float destroyDistance = 5f;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("[AutoDestroyChunk] Player not found — make sure it has tag 'Player'");
        }
    }

    void Update()
    {
        if (player == null) return;

        // If this chunk's rightmost edge is far behind the player, destroy it
        if (transform.position.x < player.position.x - destroyDistance)
        {
            Debug.Log($"[AutoDestroyChunk] Destroying chunk {gameObject.name} at X={transform.position.x}");
            Destroy(gameObject);
        }
    }
}
