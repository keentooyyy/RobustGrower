using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Chunk Settings")]
    public GameObject[] chunkPrefabs;
    public float chunkWidth = 10f;
    public int chunksAhead = 3;

    [Header("Manual Spawn Start")]
    public Vector2 spawnStartPosition = new Vector2(0f, 0f);

    [Header("Advanced Controls")]
    public Vector2 chunkOffset = Vector2.zero;
    public bool allowManualReset = false;

    private float nextSpawnX;

    void Start()
    {
        nextSpawnX = spawnStartPosition.x;

        for (int i = 0; i < chunksAhead; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        if (allowManualReset && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("[ChunkSpawner] Manual reset triggered");
            ResetSpawner();
        }

        float playerX = player.position.x;
        float threshold = playerX + (chunkWidth * chunksAhead);

        // SAFETY: Only spawn up to N chunks per frame to prevent infinite loop
        int spawnLimitPerFrame = 5;
        int chunksSpawned = 0;

        while (threshold > nextSpawnX && chunksSpawned < spawnLimitPerFrame)
        {
            SpawnNextChunk();
            chunksSpawned++;
        }
    }

    public void SpawnNextChunk()
    {
        if (chunkPrefabs.Length == 0)
        {
            Debug.LogWarning("[ChunkSpawner] No chunk prefabs assigned!");
            return;
        }

        int randIndex = Random.Range(0, chunkPrefabs.Length);
        GameObject chunkToSpawn = chunkPrefabs[randIndex];

        if (chunkToSpawn == null)
        {
            Debug.LogWarning($"[ChunkSpawner] Chunk prefab at index {randIndex} is null!");
            return;
        }

        Vector3 spawnPosition = new Vector3(nextSpawnX + chunkOffset.x, spawnStartPosition.y + chunkOffset.y, 0f);
        Instantiate(chunkToSpawn, spawnPosition, Quaternion.identity);

        nextSpawnX += chunkWidth;
    }

    public void ResetSpawner(float newStartX = 0f)
    {
        nextSpawnX = newStartX;
        Debug.Log($"[ChunkSpawner] Spawner reset to X = {nextSpawnX}");
    }

    public void SetOffset(Vector2 offset)
    {
        chunkOffset = offset;
        Debug.Log($"[ChunkSpawner] Offset set to {chunkOffset}");
    }

    public float GetNextSpawnX() => nextSpawnX;
    public void SetNextSpawnX(float newX) => nextSpawnX = newX;
}
