using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunGeon_Rogelike
{
    public class PlayerMovement : MonoBehaviour, IDamageable
    {
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Armor { get; set; }

        List<KeyCode> inputKey = new List<KeyCode>();
        List<PlayerAction> actions = new List<PlayerAction>();
        SpriteRenderer playerSpriteRenderer;
        MapManager map;
        Dictionary<Vector2, TileProperty> tileObjectMap;
        TileProperty tileProperty;

        void OnEnable()
        {
            Initialize();
        }

        void Start()
        {
            Attack = GetComponent<PlayerState>().attack;
            CheckHeartTile(tileProperty.x, tileProperty.y);
        }

        void Initialize()
        {
            map = FindObjectOfType<MapManager>();
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
            tileProperty = GetComponent<TileProperty>();
            CacheTileObjects();
        }

        void Update()
        {
            HandleInput();
            if (Input.GetKeyDown(KeyCode.Z)) UndoLastAction();
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


        void HandleInput()
        {
            Vector3[] directions = { Vector3.right, Vector3.left, Vector3.up, Vector3.down };
            KeyCode[] keys = { KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow };
            for (int i = 0; i < directions.Length; i++)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    ProcessMove(directions[i], keys[(i + 2) % 4], new Vector2Int((int)directions[i].x, (int)directions[i].y));
                    break;
                }
            }
        }

        void ProcessMove(Vector3 direction, KeyCode reverseKey, Vector2Int delta)
        {
            Vector2Int newPosition = Vector2Int.RoundToInt(transform.position) + delta;
            var nextTile = map.GetTileDataAt(newPosition.x, newPosition.y);
            TileProperty tileProperty = GetTileObjectAt(newPosition.x, newPosition.y);

            if (tileProperty && nextTile.type == TileType.Monster)
            {
                MonsterManager monsterManager = tileProperty.GetComponent<MonsterManager>();
                HandleCombat(monsterManager, nextTile.Attack);
                LogAction(Vector3.zero, false, true, Attack, nextTile.Attack, nextTile.type, newPosition, true);
            }

            if (IsTileWalkable(newPosition.x, newPosition.y))
            {
                ApplyMovement(direction, reverseKey);
                LogAction(direction, false, false, 0, 0, TileType.Empty, newPosition, false);
            }

            else if (CanPush(newPosition.x, newPosition.y, (int)direction.x, (int)direction.y))
            {
                PushTile(newPosition.x, newPosition.y, (int)direction.x, (int)direction.y);
                ApplyMovement(direction, reverseKey);
                LogAction(direction, true, false, 0, 0, nextTile.type, newPosition, false);
            }
            
            else
            {
                Debug.Log($"Blocked at ({newPosition.x}, {newPosition.y}) {nextTile.type}");
            }
        }

        void ApplyMovement(Vector3 direction, KeyCode addKey)
        {
            transform.position += direction;
            inputKey.Add(addKey);
            playerSpriteRenderer.flipX = direction == Vector3.left;
        }

        bool CanPush(int x, int y, int deltaX, int deltaY)
        {
            var nextTile = map.GetTileDataAt(x, y);
            if (nextTile != null && nextTile.isPush)
            {
                int pushToX = x + deltaX;
                int pushToY = y + deltaY;
                return IsTileWalkable(pushToX, pushToY);
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

        void UndoLastAction() 
        {
            if (actions.Count == 0) return;
            var lastAction = actions[^1];
            actions.RemoveAt(actions.Count - 1);

            // 위치 변경과 데미지 복원
            transform.position -= lastAction.PositionChange;
            UndoTilePush(lastAction);
            RecoverDamage(lastAction);

            Debug.Log("Action reversed");
        }

        void UndoTilePush(PlayerAction lastAction)
        {
            if (!lastAction.IsPush) return;
            TileProperty tile = GetTileObjectAt((int)lastAction.PositionAffected.x, (int)lastAction.PositionAffected.y);
            if (tile)
            {
                tile.transform.position -= lastAction.PositionChange;
            }
        }

        void RecoverDamage(PlayerAction lastAction)
        {
            Health += lastAction.DamageTaken;
            if (lastAction.IsAttack)
            {
                TileProperty monsterTile = GetTileObjectAt((int)lastAction.PositionAffected.x, (int)lastAction.PositionAffected.y);
                MonsterManager monsterManager = monsterTile?.GetComponent<MonsterManager>();
                
                if (monsterManager != null)
                {
                    monsterManager.TakeDamage(-lastAction.DamageDealt);  // 몬스터의 체력을 복원
                    if (lastAction.MonsterDied)
                    {
                        // 몬스터가 사망했었다면 상태를 되살립니다.
                        monsterManager.GetComponent<SpriteRenderer>().enabled = true;
                        monsterManager.Health += lastAction.DamageDealt;  // 복원할 때 사전에 저장된 체력으로 설정
                    }
                }
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
                Debug.Log("Game Over");
            else
                Debug.Log($"Player took {damage} damage, remaining health: {Health}");
        }

        bool IsTileWalkable(int x, int y)
        {
            var tile = map.GetTileDataAt(x, y);
            return tile == null || tile.isWalkable;
        }

        TileProperty GetTileObjectAt(int x, int y)
        {
            if (tileObjectMap.TryGetValue(new Vector2(x, y), out TileProperty tile))
                return tile;
            return null;
        }

        void HandleCombat(MonsterManager monsterManager, int monsterAttack)
        {
            if (!monsterManager) return;
            TakeDamage(monsterAttack);
            monsterManager.TakeDamage(Attack);
        }

        void CheckHeartTile(int x, int startY)
        {
            int maxY = map.Height;
            for (int y = startY + 1; y <= maxY; y++)
            {
                TileData tileData = map.GetTileDataAt(x, y);
                if (tileData?.type == TileType.Heart)
                {
                    SetHealth(tileData.health);
                    Debug.Log($"Player Hp is {Health}");
                    return;
                }
            }
            Debug.Log("No Heart Tile found above the player");
        }

        public void SetHealth(int newHealth)
        {
            Health = newHealth;
        }

        MonsterManager GetMonsterAt(Vector2 position) 
        {
            // 특정 위치에서 몬스터 컴포넌트를 찾아 반환
            foreach (var tile in tileObjectMap) {
                if (tile.Key == position) {
                    return tile.Value.GetComponent<MonsterManager>();
                }
            }
            return null;
        }

        void LogAction(Vector3 positionChange, bool isPush, bool isAttack, int damageDealt, int damageTaken, TileType tileType, Vector2Int positionAffected, bool monsterDied)
        {
            actions.Add(new PlayerAction {
                PositionChange = positionChange,
                IsPush = isPush,
                IsAttack = isAttack,
                DamageDealt = damageDealt,
                DamageTaken = damageTaken,
                TileTypeAffected = tileType,
                PositionAffected = positionAffected,
                MonsterDied = monsterDied
            });
        }
    }
}
