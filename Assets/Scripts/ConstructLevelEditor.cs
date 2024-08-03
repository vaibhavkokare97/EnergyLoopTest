using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConstructLevel))]
class ConstructLevelEditor : Editor
{
    private ConstructLevel _target;
    
    private void OnEnable()
    {
        _target = (ConstructLevel)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Construct"))
        {
            _target.FillTileDataList();
            _target.SnapAllTiles();
        }
            
    }
}
