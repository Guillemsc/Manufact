using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(GameGridInstance))]
public class GameGridInstanceExt :Editor
{
    [SerializeField] [HideInInspector]
    private Vector2Int new_grid_size = new Vector2Int(-1, -1);

    public override void OnInspectorGUI()
    {
        GameGridInstance myScript = (GameGridInstance)target;

        if (new_grid_size.x == -1)
            new_grid_size = myScript.GetGridSize();

        if (!Application.isPlaying)
        {
            GUILayout.Label("Grid elements ========");

            new_grid_size = EditorGUILayout.Vector2IntField("Grid size", new_grid_size);

            if (new_grid_size.x < 0)
                new_grid_size.x = 0;

            if (new_grid_size.y < 0)
                new_grid_size.y = 0;

            if (GUILayout.Button("Update grid size"))
            {
                myScript.CreateGrid(new_grid_size);
                EditorUtility.SetDirty(target);
            }
        }
        GUILayout.Label("Size ===============");

        DrawDefaultInspector();

        if (!Application.isPlaying)
        {
            GUILayout.Label("Elements ============");

            List<GameGridInstance.GridTile> tiles = myScript.GetTiles();

            for (int i = 0; i < tiles.Count; ++i)
            {
                GameGridInstance.GridTile curr_tile = tiles[i];

                string name = "[" + curr_tile.grid_pos.x + "] [" + curr_tile.grid_pos.y + "]";

                GameGridInstance.GridTileType obj = (GameGridInstance.GridTileType)EditorGUILayout.EnumPopup(name, curr_tile.type);

                if (obj != curr_tile.type)
                {
                    curr_tile.type = obj;

                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}
#endif
