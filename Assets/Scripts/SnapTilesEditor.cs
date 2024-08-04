using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SnapTiles))]
class SnapTilesEditor : Editor
{
    private SnapTiles _target;
    
    private void OnEnable()
    {
        _target = (SnapTiles)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Snap Tiles"))
        {
            _target.SnapAllTiles();
        }
            
    }
}
