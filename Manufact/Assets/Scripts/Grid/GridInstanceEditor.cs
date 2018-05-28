using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridInstanceEditor : MonoBehaviour
{
    [Header("Grid Appereance")]
    [SerializeField] private bool show_grid = true;
    [SerializeField] private bool show_grid_gos = false;

    [Header("Gird")]
    [SerializeField] private Vector2Int grid_size = new Vector2Int(10, 10);
    [SerializeField] private float slot_size = 1;

    [HideInInspector] [SerializeField] private List<GameObjectGrid> grid_gos = new List<GameObjectGrid>();

    [HideInInspector] [SerializeField] private Vector2Int curr_grid_size = Vector2Int.zero;
    [HideInInspector] [SerializeField] private float curr_slot_size = 0;
    [HideInInspector] [SerializeField] private bool curr_show_grid_gos = false;

    [System.Serializable]
    public class GameObjectGrid
    {
        public GameObject go = null;
        public Vector2Int pos = Vector2Int.zero;
        public GridSlotInstance slot_instance = null;
    }

    private void Awake()
    {
        if(Application.isPlaying)
        {
            SetInstaceInfo();
        }
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            UpdateGridSize();
        }

        UpdateShowGridGos();

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void SetInstaceInfo()
    {
        GridInstance gi = gameObject.GetComponent<GridInstance>();

        if(gi != null)
        {
            gi.SetGridInfo(grid_gos, curr_grid_size, slot_size);
        }
    }

    public List<GameObjectGrid> GetGridGos()
    {
        return grid_gos;
    }

    public float GetSlotSize()
    {
        return slot_size;
    }

    private void UpdateGridSize()
    {
        if (grid_size != curr_grid_size || slot_size != curr_slot_size)
        {
            if (grid_size.x < 0)
                grid_size.x = 0;

            if (grid_size.y < 0)
                grid_size.y = 0;

            curr_grid_size = grid_size;
            curr_slot_size = slot_size;

            for (int i = 0; i < grid_gos.Count; ++i)
            {
                DestroyImmediate(grid_gos[i].go);
            }

            grid_gos.Clear();

            for (int x = 0; x < grid_size.x; ++x)
            {
                for (int y = 0; y < grid_size.y; ++y)
                {
                    Vector2 slot_world_pos = SlotPosToWorldPos(new Vector2Int(x, y));

                    AddGridSlot(new Vector2Int(x, y), slot_world_pos);
                }
            }
        }
    }

    private void UpdateShowGridGos()
    {
        if (show_grid_gos != curr_show_grid_gos)
        {
            curr_show_grid_gos = show_grid_gos;

            for (int i = 0; i < grid_gos.Count; ++i)
            {
                if (!curr_show_grid_gos)
                    grid_gos[i].go.hideFlags = HideFlags.HideInHierarchy;
                else
                    grid_gos[i].go.hideFlags = HideFlags.None;
            }
        }
    }

    private void AddGridSlot(Vector2Int grid_pos, Vector2 slot_world_pos)
    {
        GameObject go = new GameObject();
        go.name = "Grid:[" + grid_pos.x + "]" + "[" + grid_pos.y + "]";
        go.transform.position = new Vector3(slot_world_pos.x, slot_world_pos.y, 0);
        go.transform.rotation = Quaternion.Euler(0, 0, 0);
        go.transform.localScale = new Vector3(slot_size, slot_size, 1);

        go.transform.parent = gameObject.transform;

        if (!curr_show_grid_gos)
            go.hideFlags = HideFlags.HideInHierarchy;

        GridSlotInstance gsi = go.AddComponent<GridSlotInstance>();

        GameObjectGrid go_grid = new GameObjectGrid();
        go_grid.go = go;
        go_grid.pos = grid_pos;
        go_grid.slot_instance = gsi;

        grid_gos.Add(go_grid);
    }

    private Vector2 SlotPosToWorldPos(Vector2Int slot_pos)
    {
        Vector2 ret = Vector2.zero;

        ret = new Vector2((slot_pos.x * curr_slot_size) + transform.position.x + (curr_slot_size / 2), (slot_pos.y * curr_slot_size) + transform.position.y + (curr_slot_size / 2));

        return ret;
    }

    private void OnDrawGizmos()
    {
        if (show_grid)
        {
            for (int i = 0; i < grid_gos.Count; ++i)
            {
                GameObjectGrid gog = grid_gos[i];
                GameObject curr_slot_go = gog.go;

                float half_slot_size = slot_size * 0.5f;
                float quad_x = curr_slot_go.transform.position.x - half_slot_size;
                float quad_y = curr_slot_go.transform.position.y - half_slot_size;
                float quad_w = curr_slot_go.transform.position.x + half_slot_size;
                float quad_z = curr_slot_go.transform.position.y + half_slot_size;

                Vector3 line1p1 = new Vector3(quad_x, quad_y, 0);
                Vector3 line1p2 = new Vector3(quad_x, quad_z, 0);

                Vector3 line2p1 = new Vector3(quad_x, quad_z, 0);
                Vector3 line2p2 = new Vector3(quad_w, quad_z, 0);

                Vector3 line3p1 = new Vector3(quad_w, quad_z, 0);
                Vector3 line3p2 = new Vector3(quad_w, quad_y, 0);

                Vector3 line4p1 = new Vector3(quad_w, quad_y, 0);
                Vector3 line4p2 = new Vector3(quad_x, quad_y, 0);

                Color base_color = new Color(1, 1, 1, 1);

                Color mode_color = new Color(1, 1, 1, 1);

                if (gog.slot_instance != null)
                {
                    switch (gog.slot_instance.GetSlotMode())
                    {
                        case GridInstance.SlotMode.INTERACTABLE:
                            mode_color = new Color(0.2f, 0.7f, 1.0f, 1);
                            break;
                        case GridInstance.SlotMode.NO_INTERACTABLE:
                            mode_color = new Color(1, 0, 0, 1);
                            break;
                    }

                    Gizmos.color = base_color;
                    Gizmos.DrawLine(line1p1, line1p2);
                    Gizmos.DrawLine(line2p1, line2p2);
                    Gizmos.DrawLine(line3p1, line3p2);
                    Gizmos.DrawLine(line4p1, line4p2);

                    Gizmos.color = mode_color;
                    Gizmos.DrawLine(line1p1, line3p1);
                    Gizmos.DrawLine(line2p1, line4p1);
                }
            }
        }
    }
}
