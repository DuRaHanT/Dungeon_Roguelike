using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Null, Empty, Wall, Player, Monster, Heart, Goal, PlayerStart, HeartPosition, MonsterPosition }

public class MapManager : MonoBehaviour
{
    TileType[,] tileTypes= new TileType[5,5]
    {
        { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
        { TileType.Wall, TileType.Empty, TileType.Monster, TileType.Empty, TileType.Wall },
        { TileType.Wall, TileType.Heart, TileType.Player, TileType.Goal, TileType.Wall },
        { TileType.Wall, TileType.Heart, TileType.Empty, TileType.Empty, TileType.Wall },
        { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
    };

    void OnEnable() 
    {
        PlayerMovement.OnPlayerMoved += UpdatePlayerHealth;    
    }

    void OnDisable() 
    {
        PlayerMovement.OnPlayerMoved -= UpdatePlayerHealth;
    }

    void Awake() 
    {
        InitializeMap();    
    }

    void InitializeMap()
    {

    }

    public TileType[,] GetMap() => tileTypes;

    public bool ContainsTileType(TileType type) 
    {
        for (int x = 0; x < tileTypes.GetLength(0); x++) {
            for (int y = 0; y < tileTypes.GetLength(1); y++) {
                if (tileTypes[x, y] == type) {
                    return true;
                }
            }
        }
        return false;
    }

    void UpdatePlayerHealth(Vector2Int position)
    {
        UpdateHealth(FindObjectOfType<PlayerState>(), position);
    }

    void UpdateHealth(IHealth chatacter, Vector2Int position)
    {
        int healthTiles = CountHealthTilesAbove(position);
        chatacter.SetHealth(healthTiles);
    }

    int CountHealthTilesAbove(Vector2Int position)
    {
        int count = 0;
        int x = position.x;

        for(int y = position.y +1; y < tileTypes.GetLength(1); y++)
        {
            if(tileTypes[x,y] == TileType.Heart)
            {
                count++;
            }
            else break;
        }
        return count;
    }
}
