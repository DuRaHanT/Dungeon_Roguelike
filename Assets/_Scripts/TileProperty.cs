using System;
using UnityEngine;

namespace Rog_Card
{
    public enum TileType
    {
        none,
    }

    public class TileProperty : MonoBehaviour
    {
        public TileType tileType;

        [HideInInspector] public Transform position;

        void OnEnable()
        {
            position = this.transform;
        }
    }
}