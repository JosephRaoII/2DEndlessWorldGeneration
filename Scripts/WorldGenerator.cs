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
        // Calculate the player's current chunk coordinates
        Vector2Int playerChunkCoordinates = GetChunkCoordinatesFromPosition(player.transform.position);

        // Loop through all chunks within the player's view distance
        for (int x = playerChunkCoordinates.x - viewDistance; x <= playerChunkCoordinates.x + viewDistance; x++)
        {
            for (int y = playerChunkCoordinates.y - viewDistance; y <= playerChunkCoordinates.y + viewDistance; y++)
            {
                // Generate or load the chunk if it doesn't exist
                Vector2Int chunkCoordinates = new Vector2Int(x, y);
                if (!chunkManager.IsChunkLoaded(chunkCoordinates))
                {
                    GenerateChunk(chunkCoordinates);
                }
            }
        }
    }

    private void GenerateChunk(Vector2Int chunkCoordinates)
    {
        Chunk chunk = new Chunk(chunkSize, chunkCoordinates.x, chunkCoordinates.y);
        chunk.Initialize(tilePrefab, tileParent); // Add this line to initialize the chunk's tiles

        // Generate the chunk's tiles based on noise values
        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                // Calculate the global position of the tile within the world
                Vector2Int tileCoordinates = chunk.GetTileCoordinates(x, y);
                Vector3 tilePosition = new Vector3(tileCoordinates.x, tileCoordinates.y, 0f);

                // Get the noise value for the tile position
                float noiseValue = GeneratePerlinNoise(tilePosition.x, tilePosition.y);

                // Determine the tile type based on the noise value
                TileType tileType = GetTileTypeFromNoise(noiseValue);

                // Generate the tile
                GameObject tileGO = chunk.GetTileObject(x, y);
                TileController tileController = tileGO.GetComponent<TileController>();
                tileController.SetTileType(tileType);
            }
        }
        // Add the generated chunk to the ChunkManager
        chunkManager.AddLoadedChunk(chunkCoordinates, chunk);
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
        int x = Mathf.FloorToInt(position.x / chunkSize);
        int y = Mathf.FloorToInt(position.y / chunkSize);
        return new Vector2Int(x, y);
    }
}