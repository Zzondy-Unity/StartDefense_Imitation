using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileNode
{
    public TileType tileType;
    public Vector3Int cellPos;
    public Vector3 worldPos;

    public TileNode parent;
}
