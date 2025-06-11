using UnityEngine;

[ExecuteInEditMode]
public class ChunkPreviewGizmo : MonoBehaviour
{
    [Header("Chunk Dimensions")]
    public float chunkWidth = 10f;
    public float chunkHeight = 6f;

    [Header("Preview Control")]
    public int previewChunksAhead = 3;
    public Vector2 spawnStartPosition = new Vector2(0f, 0f);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        float previewX = spawnStartPosition.x;
        for (int i = 0; i < previewChunksAhead; i++)
        {
            Vector3 center = new Vector3(previewX + chunkWidth / 2f, spawnStartPosition.y + chunkHeight / 2f, 0f);
            Vector3 size = new Vector3(chunkWidth, chunkHeight, 0.1f);

            Gizmos.DrawWireCube(center, size);

            previewX += chunkWidth;
        }

        // Optional origin indicator
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(spawnStartPosition.x, spawnStartPosition.y, 0f), 0.2f);
    }
}
