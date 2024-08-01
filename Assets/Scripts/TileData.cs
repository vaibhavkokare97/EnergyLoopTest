using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileData : TileDataBase
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_tile != null)
            {
                Debug.Log(_tile.name);
                Debug.Log("tile");
            }
        }
    }
}
