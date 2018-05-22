using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(GridCreatorManager))]
public class GridCreatorManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GridCreatorManager grid_creator = (GridCreatorManager)target;

        DrawDefaultInspector();
    }
}

#endif
