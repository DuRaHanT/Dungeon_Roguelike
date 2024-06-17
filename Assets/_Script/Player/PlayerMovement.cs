using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DunGeon_Rogelike
{
    public class PlayerMovement : MonoBehaviour, IDamageable
    {
        public int Health {get ; set;}
        public int Attack {get ; set;}
        public int Armor {get ; set;}
        List<KeyCode> inputKey = new List<KeyCode>();
        List<PlayerAction> actions = new List<PlayerAction>();
        SpriteRenderer playerSpriteRenderer;
        MapManager map;
        TileProperty tileProperty;
        PlayerState playerState;
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
            playerState = GetComponent<PlayerState>();
            map = FindObjectOfType<MapManager>();
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
            tileProperty = GetComponent<TileProperty>();
            CacheTileObjects();
            Attack = playerState.attack;
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

            TileProperty monsterObject = GetTileObjectAt(newX, newY);

            if (monsterObject != null)
            {
                MonsterManager monsterManager = monsterObject.GetComponent<MonsterManager>();
                if (monsterManager != null && nextTile.type == TileType.Monster)
                {
                    // 몬스터가 있는 경우, 플레이어와 몬스터 모두에게 데미지를 입힘
                    TakeDamage(nextTile.Attack);
                    monsterManager.TakeDamage(Attack);
                }
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
            if (actions.Count > 0)
            {
                PlayerAction lastAction = actions[^1]; // 마지막 행동 가져오기
                actions.RemoveAt(actions.Count - 1); // 마지막 행동 삭제
                
                // 위치 변경 되돌리기
                transform.position -= lastAction.PositionChange;
                
                // 푸시된 타일 되돌리기
                if (lastAction.IsPush)
                {
                    TileProperty pushedTile = GetTileObjectAt((int)lastAction.PositionAffected.x, (int)lastAction.PositionAffected.y);
                    pushedTile.transform.position -= new Vector3(lastAction.PositionChange.x, lastAction.PositionChange.y, 0);
                }

                // 받은 데미지 회복
                Health += lastAction.DamageTaken;

                // 가한 데미지 회복 (몬스터의 체력 복구)
                if (lastAction.IsAttack)
                {
                    TileProperty monsterTile = GetTileObjectAt((int)lastAction.PositionAffected.x, (int)lastAction.PositionAffected.y);
                    MonsterManager monsterManager = monsterTile.GetComponent<MonsterManager>();
                    if (monsterManager != null)
                    {
                        monsterManager.TakeDamage(-lastAction.DamageDealt); // 음수 값을 주어 체력을 회복시킴
                    }
                }

                Debug.Log("Action reversed");
            }
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
                    Debug.Log($"Player Hp is {Health}");
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

        public TileProperty GetTileObjectAt(int x, int y)
        {
            Vector2 position = new Vector2(x, y);
            if (tileObjectMap.TryGetValue(position, out TileProperty tileObject))
            {
                return tileObject;
            }
            return null;
        }

        // void LogAction(Vector3 positionChange, bool isPush, bool isAttack, int damageDealt, int damageTaken, TileType tileType, Vector2 positionAffected)
        // {
        //     actions.Add(new PlayerAction {
        //         PositionChange = positionChange,
        //         IsPush = isPush,
        //         IsAttack = isAttack,
        //         DamageDealt = damageDealt,
        //         DamageTaken = damageTaken,
        //         TileTypeAffected = tileType,
        //         PositionAffected = positionAffected
        //     });
        // }
    }
}
