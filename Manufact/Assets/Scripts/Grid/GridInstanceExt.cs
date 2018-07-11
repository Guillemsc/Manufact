using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridInstance))]
public class GridInstanceExt :Editor
{
    private Vector2Int new_grid_size = Vector2Int.zero;

    public override void OnInspectorGUI()
    {
        GridInstance myScript = (GridInstance)target;

        EditorGUILayout.Vector2IntField("Grid size", new_grid_size);

        if (GUILayout.Button("Update grid size"))
        {
            myScript.CreateGrid(new_grid_size);
        }

        GUILayout.Label("______________________________________________________________________________________________________________________________________________________________________________________________________________");

        DrawDefaultInspector();
    }
}
