using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab; // Prefab for generating new chunks
    public int chunkSize = 64; // Size of each chunk
    public int viewDistance; // Distance in chunks from the player at which chunks should be loaded

    private Dictionary<Vector2Int, Chunk> loadedChunks = new Dictionary<Vector2Int, Chunk>();
    public Vector2Int lastChunkCoord;

    private void Start()
    {
        LoadChunksAroundPlayer();
    }

    private void Update()
    {
        Vector2Int currentChunkCoord = GetChunkCoordinatesFromPosition(transform.position);

        if (currentChunkCoord != lastChunkCoord)
        {
            LoadChunksAroundPlayer();
            UnloadDistantChunks(currentChunkCoord);
        }

        lastChunkCoord = currentChunkCoord;
    }

    public void AddLoadedChunk(Vector2Int chunkCoord, Chunk chunk)
    {
        loadedChunks.Add(chunkCoord, chunk);
    }

    private void LoadChunksAroundPlayer()
    {
        Vector2Int playerChunkCoord = GetChunkCoordinatesFromPosition(transform.position);

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + y);

                if (!loadedChunks.ContainsKey(chunkCoord))
                {
                    LoadChunk(chunkCoord);
                }
            }
        }
    }

    public void ClearLoadedChunks()
    {
        foreach (var chunk in loadedChunks.Values)
        {
            Destroy(chunk.gameObject);
        }
        loadedChunks.Clear();
    }

    private void UnloadDistantChunks(Vector2Int currentChunkCoord)
    {
        List<Vector2Int> chunksToUnload = new List<Vector2Int>();

        foreach (var chunkCoord in loadedChunks.Keys)
        {
            if (Mathf.Abs(chunkCoord.x - currentChunkCoord.x) > viewDistance || Mathf.Abs(chunkCoord.y - currentChunkCoord.y) > viewDistance)
            {
                chunksToUnload.Add(chunkCoord);
            }
        }

        foreach (var chunkCoord in chunksToUnload)
        {
            UnloadChunk(chunkCoord);
        }
    }

    private void LoadChunk(Vector2Int chunkCoord)
    {
        Chunk chunk = GenerateChunk(chunkCoord);
        loadedChunks.Add(chunkCoord, chunk);
    }

    private void UnloadChunk(Vector2Int chunkCoord)
    {
        if (loadedChunks.TryGetValue(chunkCoord, out Chunk chunk))
        {
            // Destroy the chunk game object or perform any necessary cleanup
            Destroy(chunk.gameObject);
            loadedChunks.Remove(chunkCoord);
        }
    }

    private Chunk GenerateChunk(Vector2Int chunkCoord)
    {
        GameObject chunkGO = Instantiate(chunkPrefab, GetChunkPosition(chunkCoord), Quaternion.identity);
        Chunk chunk = chunkGO.GetComponent<Chunk>();
        chunk.Initialize(chunkSize, chunkCoord.x, chunkCoord.y);
        // Implement your terrain generation or tile population logic for the chunk here
        return chunk;
    }

    private Vector2Int GetChunkCoordinatesFromPosition(Vector3 position)
    {
        int chunkX = Mathf.FloorToInt(position.x / chunkSize);
        int chunkY = Mathf.FloorToInt(position.y / chunkSize);
        return new Vector2Int(chunkX, chunkY);
    }

    private Vector3 GetChunkPosition(Vector2Int chunkCoord)
    {
        float posX = chunkCoord.x * chunkSize;
        float posY = chunkCoord.y * chunkSize;
        return new Vector3(posX, posY, 0f);
    }

    public List<Chunk> GetLoadedChunks()
    {
        return new List<Chunk>(loadedChunks.Values);
    }
}
