using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour, IManager
{
    private Tilemap tilemap;
    private Dictionary<Vector3Int, TileNode> tiles = new Dictionary<Vector3Int, TileNode>();
    private TypedTile monsterSpawnTile;
    private TypedTile monsterGoalTile;
    
    public void Init()
    {
        tilemap = FindObjectOfType<Tilemap>();
        if (tilemap == null)
        {
            // 어떻게 해야하지..?
        }
        
        InitializeTileDic();
    }

    private void InitializeTileDic()
    {
        tiles.Clear();
        monsterSpawnTile = null;
        monsterGoalTile = null;
        
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; ++x)
        {
            for(int y = bounds.yMin; y < bounds.yMax; ++y)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                TileBase baseTile = tilemap.GetTile(cellPos);
                
                TileNode node = new TileNode();
                node.cellPos = new Vector3Int(x, y, 0);

                if (baseTile == null)
                {
                    node.tileType = TileType.Road;
                }
                else if (baseTile is TypedTile typed)
                {
                    node.tileType = typed.tileType;
                }
                
                Vector3 tileWorldPos = tilemap.GetCellCenterWorld(cellPos);
                node.worldPos = tileWorldPos;
                    
                tiles.Add(node.cellPos, node);

                Logger.Log($"CellPosition {cellPos} is {node.tileType}" +
                           $"\n WorldPos is {node.worldPos})");
                var test = Managers.Resource.LoadAsset<Sprite>("TestCircle");
                Instantiate(test, node.worldPos, Quaternion.identity);
            }
        }
    }
}
