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

    private Quaternion _initialRotation;

    protected override void Awake()
    {
        base.Awake();
        _initialRotation = transform.rotation;
    }

    /// <summary>
    /// Don't allow energized tile to rotate
    /// </summary>
    protected override void OnMouseDown()
    {
        transform
            .DOPunchRotation(new Vector3(0, 0, -30f), 0.2f)
            .OnComplete(delegate { transform.rotation = _initialRotation; });
    }
}
