using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMove : MonoBehaviour
{
    TileProperty tileProperty;
    MapManager mapManager;
    bool isMove;

    private void OnEnable()
    {
        tileProperty = GetComponent<TileProperty>();
        mapManager = FindObjectOfType<MapManager>();
    }

    public void HeartMovement(int x, int y)
    {

    }

}
