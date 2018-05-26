using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager grid_manager = (GridManager)target;

        if (GUILayout.Button("Create TileMap"))
        {
            grid_manager.CreateTileMap();
        }  
    }
}

#endif
