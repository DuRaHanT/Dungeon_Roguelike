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
        { TileType.Wall, TileType.Empty, TileType.MonsterPosition, TileType.Empty, TileType.Wall },
        { TileType.Wall, TileType.HeartPosition, TileType.PlayerStart, TileType.Goal, TileType.Wall },
        { TileType.Wall, TileType.HeartPosition, TileType.Empty, TileType.Empty, TileType.Wall },
        { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
    };

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

    public TileType GetTileTypeAt(Vector2Int position)
    {
        if(position.x >= 0 && position.x < tileTypes.GetLength(0) && position.y >= 0 && position.y < tileTypes.GetLength(1))
        {
            return tileTypes[position.x,position.y];
        }
        return TileType.Empty;
    }
}
