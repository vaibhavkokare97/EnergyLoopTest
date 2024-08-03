using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDataEnergized : TileDataBase
{
    public override bool energized
    {
        get
        {
            return true;
        }
    }

    /*
    protected override IEnumerator Start()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        return base.Start();
    }*/
}
