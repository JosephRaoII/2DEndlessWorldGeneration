using System.IO;
using UnityEngine;

[UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
public class SerializationManager
{
    public static void SaveChunkData(Chunk chunk, string filePath)
    {
        ChunkData chunkData = ConvertChunkToChunkData(chunk);
        string jsonData = JsonUtility.ToJson(chunkData);
        File.WriteAllText(filePath, jsonData);
    }

    public static Chunk LoadChunkData(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            ChunkData chunkData = JsonUtility.FromJson<ChunkData>(jsonData);
            ChunkManager chunkManager = JsonUtility.FromJson<ChunkManager>(jsonData);
            return ConvertChunkDataToChunk(chunkManager, chunkData);
        }
        else
        {
            Debug.LogWarning("Chunk data file does not exist: " + filePath);
            return null;
        }
    }

    private static ChunkData ConvertChunkToChunkData(Chunk chunk)
    {
        ChunkData chunkData = new ChunkData();
        chunkData.ChunkSize = chunk.ChunkSize;
        chunkData.ChunkX = chunk.ChunkX;
        chunkData.ChunkY = chunk.ChunkY;
        chunkData.TileData = new TileData[chunk.ChunkSize * chunk.ChunkSize];

        int tileIndex = 0;
        for (int x = 0; x < chunk.ChunkSize; x++)
        {
            for (int y = 0; y < chunk.ChunkSize; y++)
            {
                Tile tile = chunk.GetTile(x, y);
                TileData tileData = new TileData();
                tileData.X = x;
                tileData.Y = y;
                tileData.TileType = tile.Type;
                // Add more tile data properties as needed
                chunkData.TileData[tileIndex] = tileData;
                tileIndex++;
            }
        }

        return chunkData;
    }

    private static Chunk ConvertChunkDataToChunk(ChunkManager chunkManager, ChunkData chunkData)
    {
        Chunk chunk = new Chunk(chunkData.ChunkSize, chunkData.ChunkX, chunkData.ChunkY);

        foreach (TileData tileData in chunkData.TileData)
        {
            Tile tile = new Tile(tileData.TileType);
            int X = tileData.X;
            int Y = tileData.Y;
            // Set additional tile properties from tileData
            chunk.SetTile(tileData.X, tileData.Y, tileData.TileType);
            Vector2Int chunkLocation = new Vector2Int(tileData.X, tileData.Y);
            chunkManager.AddLoadedChunk(chunkLocation,chunk);
        }

        return chunk;
    }
}
