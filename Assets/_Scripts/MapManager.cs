using Unity.VisualScripting;
using UnityEngine;

namespace Rog_Card
{
    public class MapManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject mapParent;
        public TileProperty[,] tileProperties;
        GameObject activityPlayer;

        int width = 5, height = 4;

        void OnEnable()
        {
            Init();
        }

        void Init()
        {
            tileProperties = new TileProperty[height,width];
            
            TileProperty[] tiles = FindObjectsOfType<TileProperty>();

            foreach(var tile in tiles)
            {
                SetTileDataAt(tile);
            }
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

            PlayerMoveMent(Random.Range(0,height), 0);
        }

        public void PlayerMoveMent(int x, int y)
        {
            activityPlayer.transform.localPosition = GetTileDataAt(x,y).transform.localPosition;

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
}
