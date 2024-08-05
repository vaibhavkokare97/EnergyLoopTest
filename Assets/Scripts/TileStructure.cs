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
            case TileType.AdjacentSideConnectTile:
                return new bool[6] { false, true, true, false, false, false };
            case TileType.OneSideSkippedConnectTile:
                return new bool[6] { false, true, false, true, false, false };
            case TileType.StraightLineConnectTile:
                return new bool[6] { false, true, false, false, true, false };
            case TileType.BulbTile:
                return new bool[6] { false, false, false, false, true, false };
            case TileType.EnergyTile:
                return new bool[6] { false, false, false, false, true, false };
            default:
                return new bool[6] { false, false, false, false, false, false };
        }
    }

    public static float Clamp0360(float eulerAngles)
    {
        float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        return result;
    }
}
