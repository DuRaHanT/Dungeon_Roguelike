using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperty : MonoBehaviour
{
    public TileData tileData;
    public TileType tileType;

    public int x, y;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sprite = tileData.tileSprite;
        tileType = tileData.type;
        x = (int)transform.position.x;
        y = (int)transform.position.y;
    }

    public void ChangeTilePosition(int changeX, int changeY)
    {
        x += changeX; y += changeY;
    }
}
