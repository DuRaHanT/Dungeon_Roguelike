using UnityEngine;

[CreateAssetMenu(fileName = "New TileData", menuName = "Game/TileData")]
public class TileData : ScriptableObject {
    public TileType type;
    public Sprite tileSprite;
    public bool isWalkable;
}
