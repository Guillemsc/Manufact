using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(GridCreatorManager))]
[InitializeOnLoad]
public class GridCreatorManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GridCreatorManager grid_creator = (GridCreatorManager)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Create Grid Instance"))
        {
            grid_creator.CreateGridInstance();
        }
    }
}

#endif
