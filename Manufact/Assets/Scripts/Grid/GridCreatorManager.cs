using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridCreatorManager : Singleton<GridCreatorManager>
{
    [Header("Grid Appereance")]
    [SerializeField] private bool show_grid = true;
    [SerializeField] private Color grid_colour = new Color(1.0f, 1.0f, 1.0f, 0.2f);
    [SerializeField] private bool show_grid_gos = false;

    [Header("Gird")]
    [SerializeField] private Vector2 starting_pos = Vector2.zero;
    [SerializeField] private Vector2Int grid_size = new Vector2Int(10, 10);
    [SerializeField] private float slot_size = 1;

    [HideInInspector] [SerializeField] List<GameObject> grid = new List<GameObject>();

    private Color curr_grid_colour = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Vector2Int curr_grid_size = Vector2Int.zero;
    private float curr_slot_size = 0;
    private Vector2 curr_starting_pos = Vector2.zero;
    private bool curr_show_grid_gos = false;

    private void Awake()
    {
        InitInstance(this, gameObject);
    }

    private void Start ()
    {
        
    }

    private void Update ()
    {
        UpdateGridColour();
        UpdateGridSize();
        UpdateGridPos();
        UpdateShowGridGos();
    }

    private void UpdateGridColour()
    {
        if(grid_colour != curr_grid_colour)
        {
            grid_colour = curr_grid_colour;
        }
    }

    private void UpdateGridSize()
    {
        if(grid_size != curr_grid_size || slot_size != curr_slot_size)
        {
            if (grid_size.x < 0)
                grid_size.x = 0;

            if (grid_size.y < 0)
                grid_size.y = 0;

            curr_grid_size = grid_size;
            curr_slot_size = slot_size;

            for (int i = 0; i < grid.Count; ++i)
            {
                DestroyImmediate(grid[i]);
            }

            grid.Clear();

            for (int x = 0; x < grid_size.x; ++x)
            {
                for (int y = 0; y < grid_size.y; ++y)
                {
                    Vector2 slot_world_pos = SlotPosToWorldPos(new Vector2Int(x, y));

                    AddGridSlot(slot_world_pos);
                }
            }
        }
    }

    private void UpdateGridPos()
    {
        if(starting_pos != curr_starting_pos)
        {
            Vector2 difference = starting_pos - curr_starting_pos;

            curr_starting_pos = starting_pos;

            for (int i = 0; i < grid.Count; ++i)
            {
                grid[i].transform.position += new Vector3(difference.x, difference.y, 0);
            }
        }
    }

    private void UpdateShowGridGos()
    {
        if(show_grid_gos != curr_show_grid_gos)
        {
            curr_show_grid_gos = show_grid_gos;

            for (int i = 0; i < grid.Count; ++i)
            {
                if (!curr_show_grid_gos)
                    grid[i].hideFlags = HideFlags.HideInHierarchy;
                else
                    grid[i].hideFlags = HideFlags.None;
            }
        }
    }

    private void AddGridSlot(Vector2 slot_world_pos)
    {
        GameObject go = new GameObject();
        go.transform.position = new Vector3(slot_world_pos.x, slot_world_pos.y, 0);
        go.transform.rotation = Quaternion.Euler(0, 0, 0);
        go.transform.localScale = new Vector3(slot_size, slot_size, 1);

        go.transform.parent = gameObject.transform;

        if (!curr_show_grid_gos)
            go.hideFlags = HideFlags.HideInHierarchy;

        grid.Add(go);
    }

    private Vector2 SlotPosToWorldPos(Vector2Int slot_pos)
    {
        Vector2 ret = Vector2.zero;

        ret = new Vector2((slot_pos.x * curr_slot_size) + curr_starting_pos.x + (curr_slot_size/2), (slot_pos.y * curr_slot_size) + curr_starting_pos.y + (curr_slot_size / 2));

        return ret;
    }

    private Vector2Int WorldPosToSlotPos(Vector2 slot_world_pos)
    {
        Vector2Int ret = Vector2Int.zero;

        float pos_x = (slot_world_pos.x / slot_size) - starting_pos.x - (curr_slot_size / 2);
        float pos_y = (slot_world_pos.y / slot_size) - starting_pos.y - (curr_slot_size / 2);

        int i_pos_x = (int)pos_x;
        int i_pos_y = (int)pos_y;

        ret = new Vector2Int(i_pos_x, i_pos_y);

        return ret;
    }

    private void OnDrawGizmos()
    {
        if (show_grid)
        {
            for (int i = 0; i < grid.Count; ++i)
            {
                GameObject curr_slot_go = grid[i];

                float half_slot_size = slot_size * 0.5f;
                float quad_x = curr_slot_go.transform.position.x - half_slot_size;
                float quad_y = curr_slot_go.transform.position.y - half_slot_size;
                float quad_w = curr_slot_go.transform.position.x + half_slot_size;
                float quad_z = curr_slot_go.transform.position.y + half_slot_size;

                Vector3 center = new Vector3(curr_slot_go.transform.position.x, curr_slot_go.transform.position.y, curr_slot_go.transform.position.z);

                Vector3 line1p1 = new Vector3(quad_x, quad_y, 0);
                Vector3 line1p2 = new Vector3(quad_x, quad_z, 0);

                Vector3 line2p1 = new Vector3(quad_x, quad_z, 0);
                Vector3 line2p2 = new Vector3(quad_w, quad_z, 0);

                Vector3 line3p1 = new Vector3(quad_w, quad_z, 0);
                Vector3 line3p2 = new Vector3(quad_w, quad_y, 0);

                Vector3 line4p1 = new Vector3(quad_w, quad_y, 0);
                Vector3 line4p2 = new Vector3(quad_x, quad_y, 0);

                Gizmos.color = grid_colour;

                Gizmos.DrawLine(line1p1, line1p2);
                Gizmos.DrawLine(line2p1, line2p2);
                Gizmos.DrawLine(line3p1, line3p2);
                Gizmos.DrawLine(line4p1, line4p2);
            }
        }
    }
}
