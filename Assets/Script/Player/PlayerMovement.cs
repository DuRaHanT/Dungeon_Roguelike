using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static event Action<Vector2Int> OnPlayerMoved;
    //public static event Action<Vector2Int> OnMoveHeart;
    
    public Vector2Int playerPosition;
    [HideInInspector] public TileType[,] map;

    void Start()
    {
        map = FindObjectOfType<MapManager>().GetMap();
        playerPosition = PlayerStartPosition();
    }

    void Update() 
    {
        int horizontal = (int) Input.GetAxisRaw("Horizontal");
        int vertical = (int) Input.GetAxisRaw("Vertical");

        if (horizontal != 0) 
        {
            MovePlayer(new Vector2Int(horizontal, 0));
        } else if (vertical != 0) 
        {
            MovePlayer(new Vector2Int(0, vertical));
        }
    }

    Vector2Int PlayerStartPosition()
    {
        for(int x = 0; x < map.GetLength(0); x++)
        {
            for(int y = 0; y < map.GetLength(1); y++)
            {
                if(map[x, y] == TileType.PlayerStart)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(0, 0);
    }

    void MovePlayer(Vector2Int direction) 
    {
        Vector2Int newPosition = playerPosition + direction;
        // 맵 범위 내에 있는지 확인
        if (newPosition.x >= 0 && newPosition.x < map.GetLength(0) && newPosition.y >= 0 && newPosition.y < map.GetLength(1)) 
        {
            if (map[newPosition.x, newPosition.y] == TileType.Empty)
            {  // 움직일 위치가 비어있는지 확인
                map[playerPosition.x, playerPosition.y] = TileType.Empty;  // 이전 위치 비우기
                playerPosition += direction;  // 플레이어 위치 업데이트
                map[playerPosition.x, playerPosition.y] = TileType.Player;  // 새 위치에 플레이어 배치
                OnPlayerMoved?.Invoke(playerPosition);
            }
        }
    }

    public TileType GetTileAbovePlayer()
    {
        int x = playerPosition.x;
        int y = playerPosition.y;

        if(y + 1 < map.GetLength(1))
        {
            return map[x, y+1];
        }

        else return TileType.Null;
    }
}
