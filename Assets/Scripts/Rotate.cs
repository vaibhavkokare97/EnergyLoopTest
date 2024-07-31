using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rotate : MonoBehaviour
{
    private float _currentAngle = 0f;

    private void OnMouseDown()
    {
        transform.DORotate(new Vector3(0, 0, _currentAngle - 60f), 0.2f, RotateMode.Fast);
        _currentAngle += -60f;
    }
}
