using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMove : MonoBehaviour
{
    public Vector2Int heartPosition;
    TileType[,] map;

    void Start() 
    {
        map = FindObjectOfType<MapManager>().GetMap();
        heartPosition = new Vector2Int(2, 2);
    }


    public void MoveHeart(Vector2Int direction)
    {
        Vector2Int newPosition = heartPosition + direction;

        if (newPosition.x >= 0 && newPosition.x < map.GetLength(0) && newPosition.y >= 0 && newPosition.y < map.GetLength(1)) 
        {
            if (map[newPosition.x, newPosition.y] == TileType.Empty)
            {  // 움직일 위치가 비어있는지 확인
                
            }
        }
    }
}
