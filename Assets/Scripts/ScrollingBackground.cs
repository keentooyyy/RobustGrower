using UnityEngine;
using System.Collections.Generic;

public class ScrollingBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public GameObject[] backgroundPrefabs;

    public float spawnOffset = 2f; // Distance ahead of camera to spawn new chunk
    public float destroyDistance = 30f; // Distance behind camera to destroy chunk

    private float lastSpawnX;
    private Queue<GameObject> activeChunks = new Queue<GameObject>();

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        SpawnInitialChunk();
    }

    void Update()
    {
        float cameraX = cameraTransform.position.x;

        // Spawn new chunk if camera is nearing end of last chunk
        if (cameraX + spawnOffset > lastSpawnX)
        {
            SpawnNextChunk();
        }

        // Destroy chunks that are far behind the camera
        while (activeChunks.Count > 0)
        {
            GameObject firstChunk = activeChunks.Peek();
            if (firstChunk.transform.position.x + GetChunkWidth(firstChunk) < cameraX - destroyDistance)
            {
                Destroy(activeChunks.Dequeue());
            }
            else break;
        }
    }

    void SpawnInitialChunk()
    {
        GameObject chunk = Instantiate(GetRandomChunk(), Vector3.zero, Quaternion.identity);
        lastSpawnX = GetChunkEndX(chunk);
        activeChunks.Enqueue(chunk);
    }

    void SpawnNextChunk()
    {
        GameObject newChunk = Instantiate(GetRandomChunk(), new Vector3(lastSpawnX, 0, 0), Quaternion.identity);
        lastSpawnX = GetChunkEndX(newChunk);
        activeChunks.Enqueue(newChunk);
    }

    GameObject GetRandomChunk()
    {
        return backgroundPrefabs[Random.Range(0, backgroundPrefabs.Length)];
    }

    float GetChunkWidth(GameObject chunk)
    {
        Renderer renderer = chunk.GetComponentInChildren<Renderer>();
        return renderer != null ? renderer.bounds.size.x : 10f; // default fallback width
    }

    float GetChunkEndX(GameObject chunk)
    {
        return chunk.transform.position.x + GetChunkWidth(chunk);
    }
}
    