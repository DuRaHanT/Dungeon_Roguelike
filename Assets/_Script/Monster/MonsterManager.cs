using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunGeon_Rogelike
{
    public class MonsterManager : MonoBehaviour, IDamageable
    {
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Armor {get ; set;}
        TileProperty tileProperty;
        MapManager map;

        void OnEnable()
        {
            Init();
        }

        void Init()
        {
            map = FindObjectOfType<MapManager>();
            tileProperty = GetComponent<TileProperty>();
            Attack = 1;
        }

        void Start()
        {
            CheckHeartTile(tileProperty.x, tileProperty.y);
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if(Health <= 0)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"Monster took {amount} damage, remaining health: {Health}");
            }
        }

        void CheckHeartTile(int x, int startY)
        {
            int maxY = map.Height;
            for(int y = startY +1; y <= maxY; y++)
            {
                TileData tileData = map.GetTileDataAt(x, y);

                if (tileData == null || tileData.type != TileType.Heart)
                {
                    this.gameObject.SetActive(false);
                    break;
                }
                else
                {
                    // tileData의 타입이 Heart일 경우
                    SetHealth(x,y);
                    Debug.Log($"Monster Hp is {Health}");
                    break;
                }
            }
        }

        public void SetHealth(int x, int y)
        {
            TileData tileData = map.GetTileDataAt(x, y);
            Health = tileData.health;
        }

    }
}
