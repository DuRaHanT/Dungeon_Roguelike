using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Rog_Card
{
    public class MapManager : MonoBehaviour
    {
        public TileManager[] tiles;
        public GameObject player;

        void OnEnable()
        {
            Init();
        }

        void Init()
        {
            tiles = FindObjectsOfType<TileManager>();
        }

        void SpanwPlayer()
        {
            Instantiate(player);

            player.transform.position = tiles[0].transform.position;

        }

        public void PlayerMoveMent(int x, int z)
        {
            player.transform.position = tiles[0].transform.position;
        }
    }

}
