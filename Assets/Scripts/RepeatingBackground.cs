using UnityEngine;
using System.Collections.Generic;

public class RepeatingBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public GameObject backgroundPrefab;

    public float destroyBehindDistance = 30f;

    private float chunkWidth;
    private float lastSpawnX;
    private Queue<GameObject> spawnedChunks = new Queue<GameObject>();
    private Camera cam;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        cam = Camera.main;
        chunkWidth = GetChunkWidth(backgroundPrefab);
        SpawnInitialChunks();
    }

    void Update()
    {
        float camRightEdgeX = GetCameraRightEdgeX();

        if (lastSpawnX - camRightEdgeX < chunkWidth)
        {
            SpawnChunk();
        }

        // Destroy chunks far behind
        while (spawnedChunks.Count > 0)
        {
            GameObject chunk = spawnedChunks.Peek();
            if (chunk.transform.position.x + chunkWidth < cameraTransform.position.x - destroyBehindDistance)
            {
                Destroy(spawnedChunks.Dequeue());
            }
            else break;
        }
    }

    float GetCameraRightEdgeX()
    {
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        return cameraTransform.position.x + camHalfWidth;
    }

    void SpawnInitialChunks()
    {
        // Fill screen + extra chunk for safety
        int numInitial = Mathf.CeilToInt((GetCameraRightEdgeX() - cameraTransform.position.x) / chunkWidth) + 2;
        for (int i = 0; i < numInitial; i++)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        Vector3 spawnPos = new Vector3(lastSpawnX, 0f, 0f);
        GameObject newChunk = Instantiate(backgroundPrefab, spawnPos, Quaternion.identity);
        spawnedChunks.Enqueue(newChunk);
        lastSpawnX += chunkWidth;
    }

    float GetChunkWidth(GameObject prefab)
    {
        SpriteRenderer renderer = prefab.GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
            return renderer.bounds.size.x;

        Debug.LogWarning("No SpriteRenderer found. Defaulting width to 10.");
        return 10f;
    }
}
