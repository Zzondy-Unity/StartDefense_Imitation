using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TypedTile", menuName = "TypedTile")]
public class TypedTile : Tile
{
    public TileType tileType;
}
