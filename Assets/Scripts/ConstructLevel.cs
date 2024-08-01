using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConstructLevel : MonoBehaviour
{
    public List<TileData> tileDataList = new List<TileData>();

    public void FillTillDataList()
    {
        tileDataList.Clear();
        tileDataList = gameObject.GetComponentsInChildren<TileData>().ToList();
    }

    public void SnapAllTiles()
    {
        foreach (TileData tileData in tileDataList)
        {
            SnapToNearestAngle.SnapRotation(tileData.transform, Vector3.forward, 60f);
        }
    }
}
