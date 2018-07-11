using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelCreatorEditor))]
public class LevelCreatorEditorExt : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelCreatorEditor myScript = (LevelCreatorEditor)target;

        if (GUILayout.Button("New grid instance"))
        {
            myScript.NewGridInstance();
        }
    }
}
