using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public int ChunkSize { get; private set; } // Size of the chunk
    public int ChunkX { get; private set; } // X coordinate of the chunk
    public int ChunkY { get; private set; } // Y coordinate of the chunk
    public GameObject gameObject { get; private set; } // GameObject of the chunk

    public Tile[,] Tiles { get; private set; } // 2D array to store the tiles in the chunk

    public Chunk(int chunkSize, int chunkX, int chunkY)
    {
        ChunkSize = chunkSize;
        ChunkX = chunkX;
        ChunkY = chunkY;

        Tiles = new Tile[ChunkSize, ChunkSize];
    }

    public void SetTile(int x, int y, TileType tileType)
    {
        Tiles[x, y] = new Tile(tileType);
    }

    public Tile GetTile(int x, int y)
    {
        
        return Tiles[x, y];
    }

    public void Initialize(GameObject tilePrefab, Transform tileParent)
    {
        // Create a new game object to hold the tiles
        GameObject tilesGO = new GameObject("Tiles");
        tilesGO.transform.SetParent(transform);
        tilesGO.transform.localPosition = Vector3.zero;

        // Loop through all tile positions within the chunk
        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                // Instantiate a tile prefab and set its position and parent
                GameObject tileGO = Instantiate(tilePrefab, tilesGO.transform);
                tileGO.transform.localPosition = new Vector3(x, y, 0f);

                // Add the tile to the chunk's tile list
                tiles.Add(tileGO);
            }
        }
    }

    // Modify the existing GetTileObject method to use the tile list instead of Find()
    public GameObject GetTileObject(int x, int y)
    {
        int index = y * chunkSize + x;
        return tiles[index];
    }
    Make sure to integrate these changes into your existing scripts and test the functionality.Let me know if you encounter any issues or ha
}
