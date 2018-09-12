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
            if (GUILayout.Button("Generate new point"))
            {
                myScript.AddPathPoint(new Vector2(0, 0));

                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Clear Selection"))
            {
                selected.Clear();

                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Connect Points"))
            {
                myScript.AddPathPointConexion(selected);

                EditorUtility.SetDirty(target);
            }

            GUILayout.Space(10);

            List<EntityPathInstance.PathPoint> points = myScript.GetPathPoints();
            List<EntityPathInstance.PathPointConexions> pointConexions = myScript.GetPathPointsConections();

            for (int i = 0; i < points.Count; ++i)
            {
                EntityPathInstance.PathPoint curr_point = points[i];

                string text = "";

                if (IsSelected(curr_point))
                {
                    text += "SELECTED | ";
                }

                GUILayout.Label("___________________________");

                text += "Point: [" + i + "]";

                if (GUILayout.Button(text))
                {
                    ToggleSelection(curr_point);
                }

                EntityPathInstance.PathEntityType type = (EntityPathInstance.PathEntityType)EditorGUILayout.EnumPopup("Type", curr_point.entity.type);

                if(type != curr_point.entity.type)
                {
                    curr_point.entity.type = type;

                    EditorUtility.SetDirty(target);
                }

                float direction = EditorGUILayout.FloatField("Starting Angle", curr_point.entity.start_rotation_angle);

                if(direction != curr_point.entity.start_rotation_angle)
                {
                    curr_point.entity.start_rotation_angle = direction;

                    EditorUtility.SetDirty(target);
                }

                List<int> conexions = myScript.GetPathPointConexionsIndex(curr_point);

                for (int y = 0; y < conexions.Count; ++y)
                {
                    string c_text = "     - Connexion: ";
                    c_text += conexions[y];

                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label(c_text);

                    if (GUILayout.Button("X"))
                    {
                        EntityPathInstance.PathPoint pp1 = myScript.GetPathPointFromIndex(conexions[y]);
                        EntityPathInstance.PathPoint pp2 = curr_point;
                        myScript.RemovePathPointConexion(pp1, pp2);

                        EditorUtility.SetDirty(target);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                Vector2 new_point_pos = EditorGUILayout.Vector2Field("Position", curr_point.pos);

                if (curr_point.pos != new_point_pos)
                {
                    curr_point.pos = new_point_pos;

                    EditorUtility.SetDirty(target);
                }

                if (GUILayout.Button("Remove"))
                {
                    myScript.RemovePathPoint(curr_point);

                    EditorUtility.SetDirty(target);
                }

                EditorGUILayout.Separator();
            }
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