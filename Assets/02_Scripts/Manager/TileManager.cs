using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour, IManager
{
    private Tilemap tilemap;
    private Dictionary<Vector3Int, TypedTile> tiles = new Dictionary<Vector3Int, TypedTile>();
    private TypedTile monsterSpawnTile;
    private TypedTile monsterGoalTile;
    
    public void Init()
    {
        Tilemap _tilemap = FindObjectOfType<Tilemap>();
        if (_tilemap == null)
        {
            // 어떻게 해야하지..?
        }

        tiles.Clear();
        monsterSpawnTile = null;
        monsterGoalTile = null;
        
        BoundsInt bounds = _tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; ++x)
        {
            for(int y = bounds.yMin; y < bounds.yMax; ++y)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                TileBase baseTile = _tilemap.GetTile(cellPos);
                if (baseTile == null) continue;

                if (baseTile is TypedTile)
                {
                    
                }
            }
        }
    }
}
