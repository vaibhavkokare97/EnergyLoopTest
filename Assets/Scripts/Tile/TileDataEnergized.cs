using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileDataEnergized : TileDataBase
{
    public override bool energized
    {
        get
        {
            return true;
        }
    }

    protected override void OnMouseDown()
    {
        transform.DOShakeRotation(0.2f, 20f);
    }
}
