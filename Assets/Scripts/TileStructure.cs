using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStructure
{
    public enum TileType
    {
        TriangleConnectTile,
        AdjacentSideConnectTile,
        OneSideSkippedConnectTile,
        StraightLineConnectTile,
        BulbTile,
        EnergyTile
    }
}
