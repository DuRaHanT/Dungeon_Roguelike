using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DunGeon_Rogelike
{
    public class TileProperty : MonoBehaviour
    {
        public TileData tileData;
        public TileType tileType;

        public int x, y;

        private void OnEnable()
        {
            Init();
        }

        void Init()
        {
            GetComponent<SpriteRenderer>().sprite = tileData.tileSprite;
            tileType = tileData.type;
            x = (int)transform.position.x;
            y = (int)transform.position.y;
            TileTypeCheck(tileType);
            tileData.isWalkable = IsMoveAble(tileType);
            tileData.isPush = isPushAble(tileType);
        }

        public void ChangeTilePosition(int changeX, int changeY)
        {
            x += changeX; y += changeY;
        }

        void TileTypeCheck(TileType type)
        {
            switch(type)
            {
                case TileType.Player : 
                    this.AddComponent<PlayerMovement>();
                    this.AddComponent<PlayerState>();
                    break;
                case TileType.Heart : this.AddComponent<HeartManager>();
                    break;
                case TileType.Monster : 
                    this.AddComponent<MonsterManager>();
                    this.AddComponent<MonsterState>();
                    break;
                default : break;
            };
        }

        bool IsMoveAble(TileType type)
        {
            switch (type)
            {
                case TileType.Monster:
                case TileType.Heart:
                case TileType.Wall:
                    return false;
                default:
                    return true;
            }
        }

        bool isPushAble(TileType type)
        {
            if(TileType.Heart == type) return true;
            else return false;
        }
    }
}
