using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    private Dictionary<Vector2Int, Chunk> loadedChunks = new Dictionary<Vector2Int, Chunk>();

    public void LoadChunk(Vector2Int coordinates, Chunk chunk)
    {
        if (!loadedChunks.ContainsKey(coordinates))
        {
            loadedChunks.Add(coordinates, chunk);
            chunk.Generate();
        }
    }

    public void UnloadChunk(Vector2Int coordinates)
    {
        if (loadedChunks.ContainsKey(coordinates))
        {
            Chunk chunk = loadedChunks[coordinates];
            chunk.Destroy();
            loadedChunks.Remove(coordinates);
        }
    }

    public bool IsChunkLoaded(Vector2Int coordinates)
    {
        return loadedChunks.ContainsKey(coordinates);
    }

    public List<Vector2Int> GetLoadedChunks()
    {
        return new List<Vector2Int>(loadedChunks.Keys);
    }
}