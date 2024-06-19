using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCreator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int rows = 4;
    public int columns = 5;
    float tileSpacing = 1.5f; // 타일 간의 간격

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // 타일의 위치를 계산
                Vector3 position = new Vector3(x * (1 + tileSpacing), y * tileSpacing, y * tileSpacing);
                Quaternion rotation = Quaternion.Euler(45, 0, 0);
                GameObject newTile = Instantiate(tilePrefab, position, rotation);
                newTile.name = $"({x},{y})";
            }
        }
    }
}
