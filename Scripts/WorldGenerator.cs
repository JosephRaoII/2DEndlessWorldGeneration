using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int viewDistance;
    public int chunkSize;

    // Define the scale and offset for the Perlin noise generation
    float scale = 10f;
    public Vector2 seed = new Vector2(100f, 100f);

    private ChunkManager chunkManager;
    public GameObject player; // Reference to the player GameObject

    // Other variables and methods...

    //Tile types
    public Sprite grassSprite, stoneSprite, waterSprite, sandSprite;

    private void Start()
    {
        // Get reference to the ChunkManager and player GameObject
        chunkManager = GetComponent<ChunkManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        GenerateChunksAroundPlayer();
    }

    private void GenerateChunksAroundPlayer()
    {
        Vector2Int playerChunkCoord = GetChunkCoordinatesFromPosition(player.transform.position);

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + y);

                GenerateChunk(chunkCoord);
            }
        }
    }

    private void GenerateChunk(Vector2Int chunkCoord)
    {
        Chunk chunk = new Chunk(chunkSize, chunkCoord.x, chunkCoord.y);

        Vector2Int offset = new Vector2Int(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize);
        GenerateTiles(chunk, offset);

        chunkManager.AddLoadedChunk(chunkCoord, chunk);
    }

    private void GenerateTiles(Chunk chunk, Vector2Int offset)
    {
        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                Vector2Int tilePosition = new Vector2Int(offset.x + x, offset.y + y);

                TileType tileType = GenerateTileType(tilePosition);

                Tile tile = new Tile(tileType);
                chunk.SetTile(x, y, tileType);
            }
        }
    }

    private TileType GenerateTileType(Vector2Int tilePosition)
    {
        // Generate tile type based on noise or any other logic you prefer
        // You can use Perlin noise or any other algorithm to determine the tile type

        // Example logic using Perlin noise:
        float perlinValue = Mathf.PerlinNoise((seed.x + tilePosition.x) / scale, (seed.y + tilePosition.y) / scale);

        if (perlinValue < 0.30f)
        {
            return TileType.Water;
        }
        else if (perlinValue < 0.4f)
        {
            return TileType.Sand;
        }
        else if (perlinValue < 0.70f)
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
        int chunkX = Mathf.FloorToInt(position.x / chunkSize);
        int chunkY = Mathf.FloorToInt(position.y / chunkSize);
        return new Vector2Int(chunkX, chunkY);
    }
}