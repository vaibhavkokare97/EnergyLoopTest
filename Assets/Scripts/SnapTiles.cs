using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapTiles : MonoBehaviour
{
    public void SnapAllTiles()
    {
        List<TileDataBase> _tiledatabase = new List<TileDataBase>();
        _tiledatabase = GetComponentsInChildren<TileDataBase>().ToList();
        foreach (var tileData in _tiledatabase)
        {
            SnapToNearestAngle.SnapRotation(tileData.transform, Vector3.forward, 60f);
        }
    }
}
