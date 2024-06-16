using UnityEngine;

public enum TileType { Empty, Wall, Player, Monster, Heart, Goal}

[CreateAssetMenu(fileName = "New TileData", menuName = "Game/TileData")]
public class TileData : ScriptableObject {
    public TileType type;
    public Sprite tileSprite;
    public bool isWalkable = true;
    public bool isPush = false;
    public int health;
    public int Attack;
}
