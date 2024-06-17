using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace DunGeon_Rogelike
{   
    public enum PlayerType { None }

    public class PlayerState : MonoBehaviour 
    {
        public PlayerType playerType;
        public int armor;
        public int attack = 1;
        public int level = 0;
    }
}