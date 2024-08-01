using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConstructLevel : MonoBehaviour
{
    public Dictionary<Tile, TileData> tileDataList = new Dictionary<Tile, TileData>();

    public void FillTillDataList()
    {
        tileDataList.Clear();
        foreach (var item in gameObject.GetComponentsInChildren<TileData>())
        {
            item.SetTile();
            tileDataList.Add(item.tile, item);
        }
    }

    public void SnapAllTiles()
    {
        foreach (var tileData in tileDataList)
        {
            SnapToNearestAngle.SnapRotation(tileData.Value.transform, Vector3.forward, 60f);
        }
    }
}
