using UnityEngine;

public class ChunkController : MonoBehaviour
{
    public int chunkSize; // Size of the chunk
    public TileController tilePrefab; // Prefab for generating tiles

    private TileController[,] tiles; // Array to hold the tile objects

    public void Initialize()
    {
        // Initialize the tiles array
        tiles = new TileController[chunkSize, chunkSize];

        // Generate tiles within the chunk
        GenerateTiles();
    }

    private void GenerateTiles()
    {
        // Loop through each position in the chunk
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                // Instantiate a new tile prefab
                TileController tile = Instantiate(tilePrefab, transform);

                // Set the position of the tile
                tile.transform.position = new Vector3(x, y, 0f);

                // Store the tile in the tiles array
                tiles[x, y] = tile;
            }
        }
    }

    // Additional methods and functionality can be added as per your requirements
}