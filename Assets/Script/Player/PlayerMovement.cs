using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunGeon_Rogelike
{
    public class PlayerMovement : MonoBehaviour
    {
        List<KeyCode> inputKey = new List<KeyCode>();
        SpriteRenderer playerSpriteRenderer;
        MapManager map;
        TileProperty tileProperty;

        private void OnEnable()
        {
            map = FindObjectOfType<MapManager>();
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
            tileProperty = FindObjectOfType<TileProperty>();
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
            int newX = (int)transform.position.x + deltaX;
            int newY = (int)transform.position.y + deltaY;

            if (IsMove(newX, newY))
            {
                transform.position += direction;
                inputKey.Add(addKey);
                playerSpriteRenderer.flipX = direction == Vector3.left;
                tileProperty.ChangeTilePosition(deltaX, deltaY);
                Debug.Log($"Moved to ({newX}, {newY})");
            }
            else
            {
                Debug.Log($"Blocked at ({newX}, {newY})");
            }
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
                int deltaX = (int)moveVector.x;
                int deltaY = (int)moveVector.y;
                tileProperty.ChangeTilePosition(deltaX, deltaY);
            }
        }

        bool IsMove(int x, int y)
        {
            try
            {
                var nextTile = map.GetTileDataAt(x, y);
                if (nextTile != null)
                {
                    return nextTile.isWalkable;
                }
                return true; // 타일 정보가 없는 경우 이동 불가
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error accessing map data: {ex.Message}");
                return false;
            }

        }
    }
}
