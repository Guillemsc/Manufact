using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridInstance : MonoBehaviour
{
    private Vector2Int grid_size = Vector2Int.zero;

    private Vector2 center_pos = Vector2.zero;

    [SerializeField]
    private float tiles_size = 1.0f;

    [SerializeField]
    private Vector2 tiles_spacing = Vector2.one;

    List<GridTile> tiles = new List<GridTile>();

    public class GridTile
    {
        public Vector2 pos = Vector2.zero;
        public Vector2Int grid_pos = Vector2Int.zero;
        public GameObject go = null;
    }

    private void Start()
    {
        CreateGrid(new Vector2Int(6, 6));
    }

    private void Update()
    {
        SetGridCenter(transform.position);
        UpdateGrid();
        DebugDrawGrid();
    }

    public void CreateGrid(Vector2Int size)
    {
        grid_size = size;
    }

    public void SetTilesSpacing(Vector2 set)
    {
        tiles_spacing = set;
    }

    public void SetTilesSize(float size)
    {
        tiles_size = size;
    }

    public void AddGameObject(Vector2 tile_pos, GameObject go)
    {
        if (go != null)
        {
            for (int i = 0; i < tiles.Count; ++i)
            {
                GridTile curr_tile = tiles[i];

                if (curr_tile.grid_pos == tile_pos)
                {
                    curr_tile.go = go;
                    curr_tile.go.transform.position = curr_tile.pos;
                    break;
                }
            }
        }
    }

    public void RemoveGameObject(GameObject go)
    {
        if (go != null)
        {
            for (int i = 0; i < tiles.Count; ++i)
            {
                GridTile curr_tile = tiles[i];

                if (curr_tile.go == go)
                {
                    curr_tile.go = null;
                    break;
                }
            }
        }
    }

    public Vector2 GetGridCenter()
    {
        return center_pos;
    }

    private void SetGridCenter(Vector2 pos)
    {
        center_pos = pos;

        UpdateGrid();
    }

    private Vector2 GetGridStartingPos()
    {
        Vector2 ret = Vector2.zero;

        ret.x = center_pos.x - ((grid_size.x - 1) * tiles_size * tiles_spacing.x * 0.5f);
        ret.y = center_pos.y - ((grid_size.y - 1) * tiles_size * tiles_spacing.y * 0.5f);

        return ret;
    }

    private void UpdateGrid()
    {
        if (grid_size.x * grid_size.y == tiles.Count)
        {
            Vector2 starting_pos = GetGridStartingPos();

            for (int i = 0; i < tiles.Count; ++i)
            {
                GridTile curr_tile = tiles[i];

                curr_tile.pos.x = (curr_tile.grid_pos.x * tiles_size * tiles_spacing.x) + starting_pos.x;
                curr_tile.pos.y = (curr_tile.grid_pos.y * tiles_size * tiles_spacing.y) + starting_pos.y;
            }
        }
        else
        {
            ClearGrid();

            for (int x = 0; x < grid_size.x; ++x)
            {
                for (int y = 0; y < grid_size.y; ++y)
                {
                    GridTile tile = new GridTile();
                    tile.grid_pos = new Vector2Int(x, y);
                    tiles.Add(tile);
                }
            }
        }
    }

    private void ClearGrid()
    {
        for (int i = 0; i < tiles.Count; ++i)
        {
            GridTile curr_tile = tiles[i];

            if (curr_tile.go != null)
                Destroy(curr_tile.go);
        }

        tiles.Clear();
        grid_size = Vector2Int.zero;
    }

    private void DebugDrawGrid()
    {
        for (int i = 0; i < tiles.Count; ++i)
        {
            GridTile curr_tile = tiles[i];

            Vector3 p1 = new Vector3(curr_tile.pos.x - (tiles_size * 0.5f), curr_tile.pos.y + (tiles_size * 0.5f), 0);
            Vector3 p2 = new Vector3(curr_tile.pos.x - (tiles_size * 0.5f), curr_tile.pos.y - (tiles_size * 0.5f), 0);
            Vector3 p3 = new Vector3(curr_tile.pos.x + (tiles_size * 0.5f), curr_tile.pos.y - (tiles_size * 0.5f), 0);
            Vector3 p4 = new Vector3(curr_tile.pos.x + (tiles_size * 0.5f), curr_tile.pos.y + (tiles_size * 0.5f), 0);

            Debug.DrawLine(p1, p2);
            Debug.DrawLine(p2, p3);
            Debug.DrawLine(p3, p4);
            Debug.DrawLine(p4, p1);
        }
    }
}
