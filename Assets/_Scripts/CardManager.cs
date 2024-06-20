using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Rog_Card
{
    public class CardManager : MonoBehaviour
    {
        public GameObject player;

        public MapManager mapManager;

        void Start()
        {
            mapManager = FindObjectOfType<MapManager>();
        }

        void UseCard()
        {

        }

        void HandCard()
        {

        }
    }
}