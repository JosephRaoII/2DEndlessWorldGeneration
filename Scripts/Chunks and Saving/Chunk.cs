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

    public void Initialize(int size, int x, int y)
    {
        ChunkSize = size;
        ChunkX = x;
        ChunkY = y;
    }
}
