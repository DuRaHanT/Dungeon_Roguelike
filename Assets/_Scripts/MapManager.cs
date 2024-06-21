using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Rog_Card
{
    public class MapManager : MonoBehaviour
    {
        public GameObject player;
        public TileType[,] tileTypes;
        List<TileProperty> tileProperties;

        void OnEnable()
        {
            Init();
        }

        void Init()
        {
            tileProperties = new List<TileProperty>();

            TileProperty[] tiles = FindObjectsOfType<TileProperty>();

            foreach(var tile in tiles)
            {
                tileProperties.Add(tile);
            }

            tileTypes = new TileType[4,5];
            
            for(int i = 0; i < tileProperties.Count; i++)
            {
                SetTileDataAt(tileProperties[i].name, tileProperties[i].tileType);
            }
        }

        public void SetTileDataAt(string name, TileType tileType)
        {
            Vector2Int vec2 = ParseCoordinatesFromName(name);
            if (vec2.x >= 0 && vec2.x < tileTypes.GetLength(0) && vec2.y >= 0 && vec2.y < tileTypes.GetLength(1))
            {
                tileTypes[vec2.x, vec2.y] = tileType;
            }
            else
            {
                Debug.LogWarning($"타일 좌표 ({vec2.x}, {vec2.y})가 배열의 범위를 벗어났습니다.");
            }

        }

        void SpanwPlayer()
        {
            Instantiate(player);
        }

        public void PlayerMoveMent(int x, int y)
        {
            
        }

        Vector2Int ParseCoordinatesFromName(string name)
        {
            // 이름에서 괄호를 제거하고, 쉼표로 분리
            name = name.Trim('(', ')');
            string[] parts = name.Split(',');

            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);

            return new Vector2Int(x, y);
        }
    }
}
