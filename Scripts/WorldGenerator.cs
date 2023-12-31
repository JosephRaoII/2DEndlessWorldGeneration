using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int chunkSize = 10;
    public int viewDistance = 2;
    public int seed = 100;
    public float scale = 10f;
    public TileController tileController;

    private ChunkManager chunkManager;
    private Vector2Int currentPlayerChunk;

    private void Start()
    {
        chunkManager = GetComponent<ChunkManager>();
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        currentPlayerChunk = GetChunkCoordinatesFromPosition(transform.position);

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int chunkCoordinates = currentPlayerChunk + new Vector2Int(x, y);
                GenerateChunk(chunkCoordinates);
            }
        }
    }

    private void Update()
    {
        Vector2Int currentChunk = GetChunkCoordinatesFromPosition(transform.position);

        if (currentChunk != currentPlayerChunk)
        {
            foreach (Vector2Int chunkCoordinates in chunkManager.GetLoadedChunks())
            {
                if (Mathf.Abs(chunkCoordinates.x - currentChunk.x) > viewDistance ||
                    Mathf.Abs(chunkCoordinates.y - currentChunk.y) > viewDistance)
                {
                    chunkManager.UnloadChunk(chunkCoordinates);
                }
            }

            for (int x = -viewDistance; x <= viewDistance; x++)
            {
                for (int y = -viewDistance; y <= viewDistance; y++)
                {
                    Vector2Int chunkCoordinates = currentChunk + new Vector2Int(x, y);

                    if (!chunkManager.IsChunkLoaded(chunkCoordinates))
                    {
                        GenerateChunk(chunkCoordinates);
                    }
                }
            }

            currentPlayerChunk = currentChunk;
        }
    }

    private void GenerateChunk(Vector2Int chunkCoordinates)
    {
        Chunk chunk = new Chunk(chunkSize, chunkCoordinates.x, chunkCoordinates.y);
        GenerateTiles(chunk);

        chunkManager.LoadChunk(chunkCoordinates, chunk);
    }

    private void GenerateTiles(Chunk chunk)
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector2Int tileCoordinates = chunk.GetTileCoordinates(x, y);
                float noiseValue = GeneratePerlinNoise(tileCoordinates.x, tileCoordinates.y);
                TileType tileType = GetTileTypeFromNoise(noiseValue);
                chunk.SetTileType(x, y, tileType);
            }
        }
    }

    private float GeneratePerlinNoise(int x, int y)
    {
        float sampleX = (x + seed) / scale;
        float sampleY = (y + seed) / scale;
        return Mathf.PerlinNoise(sampleX, sampleY);
    }

    private TileType GetTileTypeFromNoise(float noiseValue)
    {
        if (noiseValue < 0.3f)
        {
            return TileType.Water;
        }
        else if (noiseValue < 0.4f)
        {
            return TileType.Sand;
        }
        else if (noiseValue < 0.7f)
        {
            return TileType.Grass;
        }
        else
        {
            return TileType.Stone;
        }
    }

    private Vector2Int GetChunkCoordinatesFromPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / chunkSize);
        int y = Mathf.FloorToInt(position.y / chunkSize);
        return new Vector2Int(x, y);
    }
}