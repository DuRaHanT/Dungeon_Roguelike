using UnityEngine;

public enum TileType { Null, Empty, Wall, Player, Monster, Heart, Goal}

[CreateAssetMenu(fileName = "New TileData", menuName = "Game/TileData")]
public class TileData : ScriptableObject {
    public TileType type;
    public Sprite tileSprite;
    public bool isWalkable;
    public bool isPush;
}
