using Unity.VisualScripting;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Rog_Card
{
    public class MapManager : MonoBehaviour
    {
        public event EventHandler<PlayerMoveEventArgs> OnPlayerMovementEvent;

        public GameObject playerPrefab;
        public GameObject mapParent;
        public TileProperty[,] tileProperties;
        GameObject activityPlayer;

        int width = 5, height = 4;

        void OnEnable()
        {
            Init();
        }

        void OnDestroy()
        {
            OnPlayerMovementEvent -= PlayerMoveMent;
        }

        void Init()
        {
            tileProperties = new TileProperty[height,width];
            
            TileProperty[] tiles = FindObjectsOfType<TileProperty>();

            foreach(var tile in tiles)
            {
                SetTileDataAt(tile);
            }

            OnPlayerMovementEvent += PlayerMoveMent;
        }

        void Start()
        {
            SpanwPlayer();
        }

        public void SetTileDataAt(TileProperty tileProperty)
        {
            Vector2Int vec2 = ParseCoordinatesFromName(tileProperty.name);
            if (vec2.x >= 0 && vec2.x < tileProperties.GetLength(0) && vec2.y >= 0 && vec2.y < tileProperties.GetLength(1))
            {
                tileProperties[vec2.x, vec2.y] = tileProperty;
            }
            else
            {
                Debug.LogWarning($"타일 좌표 ({vec2.x}, {vec2.y})가 배열의 범위를 벗어났습니다.");
            }
        }

        public TileProperty GetTileDataAt(int x, int y)
        {
            return tileProperties[x, y];
        }

        void SpanwPlayer()
        {
            activityPlayer = Instantiate(playerPrefab);

            activityPlayer.transform.SetParent(mapParent.transform, false);

            OnPlayerMove(Random.Range(0,height), 0);
        }

        public void OnPlayerMove(int x, int y)
        {
            Debug.Log("Run Event");
            OnPlayerMovementEvent?.Invoke(this, new PlayerMoveEventArgs(x, y));
        }

        void PlayerMoveMent(object sender, PlayerMoveEventArgs e)
        {
            int x = Mathf.Clamp(e.X, 0, height);
            int y = Mathf.Clamp(e.Y, 0, width);

            activityPlayer.transform.localPosition = GetTileDataAt(x,y).transform.localPosition;

            var state = activityPlayer.GetComponent<PlayerState>();

            state.currentX = x;
            state.currentY = y;

            Debug.Log($"player position is {activityPlayer.transform.localPosition}");

            Debug.Log($"player is {x}, {y}");
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

    #region Event
    public class PlayerMoveEventArgs : EventArgs
    {
        public int X { get; }
        public int Y { get; }

        public PlayerMoveEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    #endregion
}