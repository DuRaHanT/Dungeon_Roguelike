using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DunGeon_Rogelike
{
    public class PlayerAction
    {
        public Vector3 PositionChange { get; set; }
        public bool IsPush { get; set; }
        public bool IsAttack { get; set; }
        public int DamageDealt { get; set; }
        public int DamageTaken { get; set; }
        public TileType TileTypeAffected { get; set; }
        public Vector2 PositionAffected { get; set; }         
        public bool MonsterDied { get; set; }
    }
}