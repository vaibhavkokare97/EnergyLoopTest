using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDataBulb : TileDataBase
{
    protected override void OnTileEnergize(bool onEnergize)
    {
        base.OnTileEnergize(onEnergize);
        GameManager.Instance.energizedBulbNodeCount += (onEnergize) ? 1 : -1;
    }
}
