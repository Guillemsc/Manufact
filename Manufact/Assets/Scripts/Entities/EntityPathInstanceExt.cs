using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(EntityPathInstance))]
public class EntityPathInstanceExt : Editor
{
    private List<EntityPathInstance.PathPoint> selected = new List<EntityPathInstance.PathPoint>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EntityPathInstance myScript = (EntityPathInstance)target;

        if (!Application.isPlaying)
        {
            if(GUILayout.Button("Generate new point"))
            {
                myScript.AddPathPoint(new Vector2(0, 0));

                EditorUtility.SetDirty(target);
            }
        }

        if (GUILayout.Button("Clear Selection"))
        {
            selected.Clear();

            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Connect Points"))
        {
            myScript.AddPathPointConexion(selected[0], selected[1]);

            EditorUtility.SetDirty(target);
        }

        GUILayout.Space(20);

        List<EntityPathInstance.PathPoint> points = myScript.GetPathPoints();
        List<EntityPathInstance.PathPointConexions> pointConexions = myScript.GetPathPointsConections();

        for(int i = 0; i < points.Count; ++i)
        {
            EntityPathInstance.PathPoint curr_point = points[i];

            string text = "";

            if(IsSelected(curr_point))
            {
                text += "SELECTED | ";
            }

            text += "Point: [" + i + "]     ====================";

            if (GUILayout.Button(text))
            {
                ToggleSelection(curr_point);
            }

            List<int> conexions = myScript.GetPathPointConexions(curr_point);

            for (int y = 0; y < conexions.Count; ++y)
            {
                string c_text = "     - ";
                c_text += conexions[y];

                GUILayout.Label(c_text);
            }

            Vector2 last_point_pos = curr_point.pos;
            curr_point.pos = EditorGUILayout.Vector2Field("Position", curr_point.pos);

            if(curr_point.pos != last_point_pos)
                EditorUtility.SetDirty(target);

            if (GUILayout.Button("Remove"))
            {
                myScript.RemovePathPoint(curr_point);

                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.Separator();
        }
    }

    private void Select(EntityPathInstance.PathPoint sel)
    {
        if(!IsSelected(sel))
            selected.Add(sel);
    }

    private void Deselect(EntityPathInstance.PathPoint sel)
    {
        selected.Remove(sel);
    }

    private void ToggleSelection(EntityPathInstance.PathPoint sel)
    {
        if(IsSelected(sel))
            Deselect(sel);
        else
            Select(sel); 
    }

    private bool IsSelected(EntityPathInstance.PathPoint sel)
    {
        bool ret = false;

        for (int i = 0; i < selected.Count; ++i)
        {
            if(selected[i] == sel)
            {
                ret = true;
                break;
            }
        }

        return ret;
    }
}

#endif