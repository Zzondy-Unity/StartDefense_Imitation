using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager
{
    private Tilemap tilemap;
    private Dictionary<Vector3Int, TileNode> tiles = new Dictionary<Vector3Int, TileNode>();
    private TileNode monsterSpawnTile;
    private TileNode monsterGoalTile;
    
    public TileManager(Tilemap tilemap)
    {
        this.tilemap = tilemap;
        this.tilemap.CompressBounds();   // 사용된 셀만 감싸도록 bounds를 축소하는 함수
        
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
                    if (typed.tileType == TileType.Goal)
                    {
                        monsterGoalTile = node;
                    }
                    else if (typed.tileType == TileType.Spawn)
                    {
                        monsterSpawnTile = node;
                    }
                }
                
                Vector3 tileWorldPos = tilemap.GetCellCenterWorld(cellPos);
                node.worldPos = tileWorldPos;
                    
                tiles.Add(node.cellPos, node);

                // Logger.Log($"CellPosition {cellPos} is {node.tileType}" +
                //            $"\n WorldPos is {node.worldPos})");
                // var test = Managers.Resource.LoadAsset<GameObject>("Circle");
                // var sr = test.GetComponent<SpriteRenderer>();
                // switch (node.tileType)
                // {
                //     case TileType.Road:
                //         sr.color = Color.white;
                //         break;
                //     case TileType.Spawn:
                //         sr.color = Color.black;
                //         break;
                //     case TileType.Goal:
                //         sr.color = Color.blue;
                //         break;
                //     case TileType.Block:
                //         sr.color = Color.red;
                //         break;
                //     case TileType.Normal:
                //         sr.color = Color.yellow;
                //         break;
                //     case TileType.Wall:
                //         sr.color = Color.green;
                //         break;
                //     case TileType.FixTile:
                //         sr.color = Color.magenta;
                //         break;
                // }
                // Instantiate(test, node.worldPos, Quaternion.identity);
            }
        }
    }
}
