using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public string saveFolderPath = "SaveData";
    public string fileExtension = ".json";

    private ChunkManager chunkManager;
    public GameObject CameraLocation;

    private void Awake()
    {
        chunkManager = GetComponent<ChunkManager>();
    }

    public void SaveGame()
    {
        // Create the save folder if it doesn't exist
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        // Save each loaded chunk
        foreach (Chunk chunk in chunkManager.GetLoadedChunks())
        {
            string filePath = GetChunkFilePath(chunk.ChunkX, chunk.ChunkY);
            SerializationManager.SaveChunkData(chunk, filePath);
        }

        Debug.Log("Game saved.");
    }

    public void LoadGame(ChunkManager chunkManager)
    {
        // Clear the current loaded chunks
        chunkManager.ClearLoadedChunks();

        // Load each chunk from the save files
        string[] saveFiles = Directory.GetFiles(saveFolderPath, "*" + fileExtension);
        foreach (string filePath in saveFiles)
        {
            Chunk chunk = SerializationManager.LoadChunkData(filePath);
            if (chunk != null)
            {
                
                chunkManager.AddLoadedChunk(chunkManager.lastChunkCoord , chunk);
            }
        }

        Debug.Log("Game loaded.");
    }

    private string GetChunkFilePath(int chunkX, int chunkY)
    {
        string fileName = "chunk_" + chunkX + "_" + chunkY + fileExtension;
        return Path.Combine(saveFolderPath, fileName);
    }
}