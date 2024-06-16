using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DunGeon_Rogelike
{
    public class HeartManager : MonoBehaviour
    {
        TileProperty tileProperty;
        int health = 3;

        void OnEnable()
        {
            Init();
        }

        void Init()
        {
            tileProperty = GetComponent<TileProperty>();
            tileProperty.tileData.health = health;
        }
    }
}
