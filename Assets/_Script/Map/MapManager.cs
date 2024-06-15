using UnityEngine;


public class MapManager : MonoBehaviour
{
    int minX = -8;
    int maxX = 8;
    int minY = -8;
    int maxY = 8;
    public TileData[,] tileDataArray;

    private int width;
    private int height;
    private int offsetX;
    private int offsetY;

    private void OnEnable()
    {
        InitializeMap();
    }

    void InitializeMap()
    {
        offsetX = -minX;
        offsetY = -minY;
        width = maxX - minX + 1;
        height = maxY - minY + 1;

        tileDataArray = new TileData[width, height];

        TileProperty[] allTiles = FindObjectsOfType<TileProperty>();
        foreach (var tile in allTiles)
        {
            int x = Mathf.FloorToInt(tile.transform.position.x);
            int y = Mathf.FloorToInt(tile.transform.position.y);
            SetTileData(x, y, tile.tileData); // 실제 좌표를 기반으로 타일 데이터 객체 저장
        }
    }

    public void SetTileData(int x, int y, TileData tileData)
    {
        int adjustedX = x + offsetX;
        int adjustedY = y + offsetY;
        if (adjustedX >= 0 && adjustedY >= 0 && adjustedX < width && adjustedY < height)
        {
            tileDataArray[adjustedX, adjustedY] = tileData;
        }
    }

    public TileData GetTileDataAt(int x, int y)
    {
        int adjustedX = x + offsetX;
        int adjustedY = y + offsetY;
        if (adjustedX >= 0 && adjustedY >= 0 && adjustedX < width && adjustedY < height)
        {
            return tileDataArray[adjustedX, adjustedY];
        }
        return null; // 범위를 벗어나면 null 반환
    }

    public void ReSetTileType(int x, int y, TileType newType)
    {
        int adjustedX = x + offsetX;
        int adjustedY = y + offsetY;
        if (adjustedX >= 0 && adjustedY >= 0 && adjustedX < width && adjustedY < height)
        {
            TileData tileData = tileDataArray[adjustedX, adjustedY];
            if (tileData != null) // 이미 타일 데이터가 할당되어 있는지 확인
            {
                tileData.type = newType; // 타일 타입 변경
            }
            else
            {
                // 타일 데이터가 없으면 새로 생성하거나 다른 처리를 할 수 있습니다.
                tileData = new TileData(); // 새로운 TileData 인스턴스 생성
                tileData.type = newType;
                tileDataArray[adjustedX, adjustedY] = tileData; // 배열에 새로 할당
            }
        }
    }

}
