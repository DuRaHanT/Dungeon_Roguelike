using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMove : MonoBehaviour
{
    public Vector2Int heartPosition;
    TileType[,] map;
    MapManager mapManager;

    void Start() 
    {
        map = FindObjectOfType<MapManager>().GetMap();
        Vector2Int myPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        TileType myTileType = mapManager.GetTileTypeAt(myPosition);

        if(myTileType == TileType.HeartPosition)
        {
            heartPosition = myPosition;
            map[heartPosition.x, heartPosition.y] = TileType.Heart;
        }
    }
}
