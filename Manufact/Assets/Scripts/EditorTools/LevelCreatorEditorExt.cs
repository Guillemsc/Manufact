using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(LevelCreatorEditor))]
public class LevelCreatorEditorExt : Editor
{
    public override void OnInspectorGUI()
    {
        LevelCreatorEditor myScript = (LevelCreatorEditor)target;

        if (GUILayout.Button("New grid instance"))
        {
            myScript.NewGridInstance();
        }

        if (GUILayout.Button("New path instance"))
        {
            myScript.NewPathInstance();
        }

        DrawDefaultInspector();
    }
}

#endif
