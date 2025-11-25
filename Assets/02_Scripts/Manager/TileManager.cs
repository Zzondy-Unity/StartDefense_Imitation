using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileManager
{
    private GameSceneManager manager;
    
    private Tilemap tilemap;
    private Dictionary<Vector3Int, TileNode> tiles = new Dictionary<Vector3Int, TileNode>();
    private Dictionary<TileType, TypedTile> tileTypes = new Dictionary<TileType, TypedTile>();
    private TileNode monsterSpawnTile;
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

    public void OnGameEnd(bool isWin)
    {
        
    }
    
    private void InitializeTileDic()
    {
        tiles.Clear();
        monsterSpawnTile = null;

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
                    if (typed.tileType == TileType.Spawn)
                    {
                        monsterSpawnTile = node;
                    }
                    
                    if (!tileTypes.ContainsKey(node.tileType))
                    {
                        tileTypes.Add(node.tileType, typed);
                    }
                }

                Vector3 tileWorldPos = tilemap.GetCellCenterWorld(cellPos);
                node.worldPos = tileWorldPos;

                tiles.Add(node.cellPos, node);
            }
        }
    }

    public TileNode GetTileNode(Vector3 worldPos)
    {
        if (tilemap == null)
        {
            var manager = GameManager.Scene.curSceneManager as GameSceneManager;
            this.tilemap = manager?.Tilemap;
        }
        
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        if (tiles.TryGetValue(cellPos, out TileNode node))
        {
            return node;
        }
        return null;
    }

    public Vector3[] GetMonsterPath()
    {
        return monsterPath;
    }

    public Vector3 GetSpawnPosition()
    {
        return monsterSpawnTile.worldPos;
    }

    public void ChangeTile(TileNode curTile, TileType type)
    {
        if (tiles.TryGetValue(curTile.cellPos, out TileNode node))
        {
            tiles[curTile.cellPos].tileType = type;
            tilemap.SetTile(curTile.cellPos, tileTypes[type]);
        }
    }
}