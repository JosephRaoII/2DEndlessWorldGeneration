using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileController : MonoBehaviour
{
    public Sprite waterSprite;
    public Sprite sandSprite;
    public Sprite grassSprite;
    public Sprite stoneSprite;

    private Dictionary<TileType, Sprite> tileSprites;

    private void Awake()
    {
        tileSprites = new Dictionary<TileType, Sprite>
        {
            { TileType.Water, waterSprite },
            { TileType.Sand, sandSprite },
            { TileType.Grass, grassSprite },
            { TileType.Stone, stoneSprite }
        };
    }

    public void UpdateTile(GameObject tileGO, TileType tileType)
    {
        SpriteRenderer spriteRenderer = tileGO.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = tileSprites[tileType];
    }
}