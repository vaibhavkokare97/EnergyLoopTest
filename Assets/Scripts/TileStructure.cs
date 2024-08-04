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

    public static bool[] ConnectedSides(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.TriangleConnectTile:
                return new bool[6] { false, true, false, true, false, true };
                break;
            case TileType.AdjacentSideConnectTile:
                return new bool[6] { false, true, true, false, false, false };
                break;
            case TileType.OneSideSkippedConnectTile:
                return new bool[6] { false, true, false, true, false, false };
                break;
            case TileType.StraightLineConnectTile:
                return new bool[6] { false, true, false, false, true, false };
                break;
            case TileType.BulbTile:
                return new bool[6] { false, false, false, false, true, false };
                break;
            case TileType.EnergyTile:
                return new bool[6] { false, false, false, false, true, false };
                break;
            default:
                return new bool[6] { false, false, false, false, false, false };
                break;
        }
    }
}
