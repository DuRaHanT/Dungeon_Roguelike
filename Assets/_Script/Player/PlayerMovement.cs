using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunGeon_Rogelike
{
    public class PlayerMovement : MonoBehaviour, IDamageable
    {
        public int Health {get ; set;}
        List<KeyCode> inputKey = new List<KeyCode>();
        SpriteRenderer playerSpriteRenderer;
        MapManager map;
        TileProperty tileProperty;
        Dictionary<Vector2, TileProperty> tileObjectMap;

        void OnEnable()
        {
            Init();
        }

        void Start()
        {
            CheckHeartTile(tileProperty.x, tileProperty.y);
        }

        void Init()
        {
            map = FindObjectOfType<MapManager>();
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
            tileProperty = GetComponent<TileProperty>();
            CacheTileObjects();
        }

        private void CacheTileObjects()
        {
            tileObjectMap = new Dictionary<Vector2, TileProperty>();
            foreach (var tile in FindObjectsOfType<TileProperty>())
            {
                Vector2 pos = new Vector2(Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.y));
                tileObjectMap[pos] = tile;
            }
        }

        void Update()
        {
            HandleInput();
            if (Input.GetKeyDown(KeyCode.Z)) BackMove();
        }

        void HandleInput()
        {
            Vector3[] directions = { Vector3.right, Vector3.left, Vector3.up, Vector3.down };
            KeyCode[] keys = { KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow };
            KeyCode[] reverseKeys = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.UpArrow };
            int[,] deltas = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

            for (int i = 0; i < directions.Length; i++)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    MoveDirection(directions[i], reverseKeys[i], deltas[i, 0], deltas[i, 1]);
                    break; // Only handle one key per frame to avoid conflicts
                }
            }
        }

        void MoveDirection(Vector3 direction, KeyCode addKey, int deltaX, int deltaY)
        {
            int newX = Mathf.RoundToInt(transform.position.x + deltaX);
            int newY = Mathf.RoundToInt(transform.position.y + deltaY);

            var nextTile = map.GetTileDataAt(newX, newY);

            if (nextTile != null && nextTile.type == TileType.Monster)
            {
                // 몬스터가 있는 경우, 데미지를 입힙니다.
                TakeDamage(nextTile.Attack);
            }

            // 타일이 이동 가능한지 여부를 확인하고 이동 처리
            if (IsMove(newX, newY))
            {
                ApplyMovement(direction, addKey, newX, newY);
                CheckHeartTile(newX, newY);
            }
            else if (CanPush(newX, newY, deltaX, deltaY))
            {
                PushTile(newX, newY, deltaX, deltaY);
                ApplyMovement(direction, addKey, newX, newY);
                CheckHeartTile(newX, newY);
            }
            else
            {
                Debug.Log($"Blocked at ({newX}, {newY}) {nextTile.type}");
            }
        }

        void ApplyMovement(Vector3 direction, KeyCode addKey, int x, int y)
        {
            transform.position += direction;
            inputKey.Add(addKey);
            playerSpriteRenderer.flipX = direction == Vector3.left;
            Debug.Log($"Moved to ({x}, {y})");
        }

        void BackMove()
        {
            if (inputKey.Count <= 0) return;
            KeyCode lastKey = inputKey[^1]; // Using C# 8.0 syntax for last element
            inputKey.RemoveAt(inputKey.Count - 1);
            ReactToKey(lastKey);
        }

        void ReactToKey(KeyCode key)
        {
            Vector3 moveVector = key switch
            {
                KeyCode.RightArrow => Vector3.right,
                KeyCode.LeftArrow => Vector3.left,
                KeyCode.UpArrow => Vector3.up,
                KeyCode.DownArrow => Vector3.down,
                _ => Vector3.zero
            };

            if (moveVector != Vector3.zero)
            {
                transform.position += moveVector;
                playerSpriteRenderer.flipX = key == KeyCode.LeftArrow;
            }
        }

        bool IsMove(int x, int y)
        {
            var nextTile = map.GetTileDataAt(x, y);
            if(nextTile == null) return true;
            else return nextTile.isWalkable;
        }

        bool CanPush(int x, int y, int deltaX, int deltaY)
        {
            var nextTile = map.GetTileDataAt(x, y);
            if (nextTile != null && nextTile.isPush)
            {
                int pushToX = x + deltaX;
                int pushToY = y + deltaY;
                return IsMove(pushToX, pushToY);
            }
            return false;
        }

        void PushTile(int x, int y, int deltaX, int deltaY)
        {
            var pushedTile = map.GetTileDataAt(x, y);
            Vector2 oldPos = new Vector2(x, y);
            Vector2 newPos = new Vector2(x + deltaX, y + deltaY);

            if (tileObjectMap.ContainsKey(oldPos))
            {
                TileProperty tileObj = tileObjectMap[oldPos];
                tileObjectMap.Remove(oldPos);
                tileObjectMap[newPos] = tileObj;
                tileObj.transform.position = new Vector3(x + deltaX, y + deltaY, 0);
            }

            map.SetTileData(x + deltaX, y + deltaY, pushedTile);
            map.ReSetTileType(x, y, TileType.Empty); 
        }

        void CheckHeartTile(int x, int startY)
        {
            int maxY = map.Height;
            for(int y = startY +1; y <= maxY; y++)
            {
                TileData tileData = map.GetTileDataAt(x, y);

                if (tileData == null || tileData.type != TileType.Heart)
                {
                    // tileData가 null이거나 Heart 타입이 아닌 경우 Game Over 출력  
                    Debug.Log("Game Over");
                    break;
                }
                else
                {
                    // tileData의 타입이 Heart일 경우
                    SetHealth(x,y);
                    Debug.Log($"Hp is {Health}");
                    break;
                }
            }
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if(Health <= 0)
            {
                Debug.Log("Game Oveer");
            }
            else
            {
                Debug.Log($"Player took {amount} damage, remaining health: {Health}");
            }
        }

        public void SetHealth(int x, int y)
        {
            TileData tileData = map.GetTileDataAt(x, y);
            Health = tileData.health;
        }
    }
}
