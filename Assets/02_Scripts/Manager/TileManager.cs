using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileManager
{
    private GameSceneManager manager;
    
    private Tilemap tilemap;
    private Dictionary<Vector3Int, TileNode> tiles = new Dictionary<Vector3Int, TileNode>();
    private TileNode monsterSpawnTile;
    private TileNode monsterGoalTile;
    private Vector3[] monsterPath;
    
    public TileManager(Tilemap tilemap, Vector3[] waypoints, GameSceneManager manager)
    {
        this.manager = manager;
        this.tilemap = tilemap;
        this.tilemap.CompressBounds(); // 사용된 셀만 감싸도록 bounds를 축소하는 함수
        monsterPath = waypoints;

        InitializeTileDic();
        // CalculateWayPoints();
    }
    
    private void InitializeTileDic()
    {
        tiles.Clear();
        monsterSpawnTile = null;
        monsterGoalTile = null;

        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; ++x)
        {
            for (int y = bounds.yMin; y < bounds.yMax; ++y)
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

    public Vector3[] GetMonsterPath()
    {
        return monsterPath;
    }

    public Vector3 GetSpawnPosition()
    {
        return monsterSpawnTile.worldPos;
    }

    #region <<<<<<<< BFS를 이용한 최단거리 계산 >>>>>>>>
    
    // public Stack<Vector3> GetMonsterPath()
    // {
    //     if (monsterPath.Count == 0)
    //     {
    //         CalculateWayPoints();
    //     }
    //
    //     return monsterPath;
    // }
    //
    // private readonly Vector3Int[] neighborOffset =
    // {
    //     new Vector3Int(1, 0, 0),
    //     new Vector3Int(-1, 0, 0),
    //     new Vector3Int(0, 1, 0),
    //     new Vector3Int(0, -1, 0),
    // };
    //
    // private void CalculateWayPoints()
    // {
    //     // 길찾기 시작하기
    //     monsterPath.Clear();
    //
    //     if (monsterSpawnTile == null || monsterGoalTile == null)
    //     {
    //         Logger.ErrorLog(
    //             $"[TileManager] monsterSpawnTile : {monsterSpawnTile}\n monsterGoalTile : {monsterGoalTile}");
    //         return;
    //     }
    //
    //     Queue<TileNode> currentPath = new Queue<TileNode>();
    //     HashSet<TileNode> visited = new HashSet<TileNode>();
    //
    //     currentPath.Enqueue(monsterSpawnTile);
    //     visited.Add(monsterSpawnTile);
    //
    //     bool isFound = false;
    //
    //     while (currentPath.Count > 0)
    //     {
    //         var curNode = currentPath.Dequeue();
    //         if (curNode.tileType == TileType.Goal)
    //         {
    //             isFound = true;
    //             break;
    //         }
    //
    //         foreach (var neighbor in GetNeighbors(curNode))
    //         {
    //             if (visited.Contains(neighbor))
    //             {
    //                 continue;
    //             }
    //
    //             visited.Add(neighbor);
    //             currentPath.Enqueue(neighbor);
    //             neighbor.parent = curNode;
    //         }
    //     }
    //
    //     if (!isFound)
    //     {
    //         Logger.ErrorLog($"[TileManager] 길을 찾을 수 없습니다.");
    //         return;
    //     }
    //
    //     Stack<Vector3> path = new Stack<Vector3>();
    //     TileNode nodePtr = monsterGoalTile;
    //
    //     while (nodePtr != null)
    //     {
    //         path.Push(nodePtr.worldPos);
    //         nodePtr = nodePtr.parent;
    //     }
    //
    //     monsterPath = path;
    // }
    //
    // private IEnumerable<TileNode> GetNeighbors(TileNode cur)
    // {
    //     foreach (var offset in neighborOffset)
    //     {
    //         var neighborPos = cur.cellPos + offset;
    //         if (tiles.TryGetValue(neighborPos, out TileNode neighbor))
    //         {
    //             if (IsWalkable(neighbor))
    //             {
    //                 yield return neighbor;
    //             }
    //         }
    //     }
    // }
    //
    // private bool IsWalkable(TileNode neighbor)
    // {
    //     return neighbor.tileType == TileType.Road
    //            || neighbor.tileType == TileType.Spawn
    //            || neighbor.tileType == TileType.Goal;
    // }

    #endregion
}