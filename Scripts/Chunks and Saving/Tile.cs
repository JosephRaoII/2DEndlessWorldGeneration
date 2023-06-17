public enum TileType
{
    Grass,
    Water,
    Sand,
    Stone,
    Snow,
    // Add more tile types as needed
}

public class Tile
{
    public TileType Type { get; set; }
    // Add more properties as needed for tile information

    public Tile(TileType type)
    {
        Type = type;
    }
}