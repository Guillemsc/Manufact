using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(EntityPathInstance))]
public class EntityPathInstanceExt : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EntityPathInstance myScript = (EntityPathInstance)target;

        if (!Application.isPlaying)
        {
            if(GUILayout.Button("Generate new point"))
            {
                myScript.AddPathPoint(new Vector2(0, 0));
            }
        }
    }
}

#endif