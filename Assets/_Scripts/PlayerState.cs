using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Rog_Card
{
    public enum PlayerType
    {
        Warrior,
        Wizard,
        Assassin
    }

    public class PlayerState : MonoBehaviour, IAbility
    {
        MapManager mapManager;
        PlayerType playerType;

        public int health { get; set; }
        public int attack { get; set; }
        public int moveRange { get; set; }
        public int currentX { get; set; }
        public int currentY { get; set; }

        void Awake ()
        {
            mapManager = FindObjectOfType<MapManager>();
            SetState();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                mapManager.OnPlayerMove(currentX + 1, currentY);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                mapManager.OnPlayerMove(currentX - 1, currentY);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                mapManager.OnPlayerMove(currentX, currentY - 1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                mapManager.OnPlayerMove(currentX, currentY + 1);
            }

        }

        void SetState()
        {
            switch(playerType)
            {
                case PlayerType.Warrior :
                    health = 10;
                    attack = 2;
                    moveRange = 1;
                return;
                case PlayerType.Wizard :
                    health = 5;
                    attack = 4;
                    moveRange = 1;
                return;
                case PlayerType.Assassin :
                    health = 7;
                    attack = 3;
                    moveRange = 2;
                return;
                default : return;
            }
        }
    }
}
