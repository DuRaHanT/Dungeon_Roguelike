using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rog_Card
{
    public class PlayerState : MonoBehaviour
    {
        MapManager mapManager;

        void Awake ()
        {
            mapManager = FindObjectOfType<MapManager>();
        }

        void Start ()
        {
            this.transform.position = mapManager.tiles[0].transform.position;
        }

        void Update()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            
        }
    }
}
