using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace DunGeon_Rogelike
{
    public enum MonsterType { orc }

    public class MonsterState : MonoBehaviour 
    {
        public MonsterType monsterType;
        public int armor;
        public int attack = 1;
        public bool isDead = false;
        public int Health { get; set; }
        public Vector2 Position { get; set; }

    }
}