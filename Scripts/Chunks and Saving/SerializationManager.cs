using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]

public static class SerializationManager
{
    public static void SaveChunkData(Chunk chunk, string savePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Create(savePath);
        formatter.Serialize(fileStream, chunk.GetTileTypes());
        fileStream.Close();
    }

    public static bool LoadChunkData(Chunk chunk, string savePath)
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(savePath, FileMode.Open);
            chunk.SetTileTypes((TileType[,])formatter.Deserialize(fileStream));
            fileStream.Close();
            return true;
        }
        return false;
    }
}
