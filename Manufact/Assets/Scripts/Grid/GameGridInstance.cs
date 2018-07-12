using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameGridInstance : MonoBehaviour
{
    [SerializeField] [HideInInspector]
    private Vector2Int grid_size = Vector2Int.zero;

    private Vector2 center_pos = Vector2.zero;

    [SerializeField]
    private float tiles_size = 1.0f;

    [SerializeField]
    private Vector2 tiles_spacing = Vector2.one;

    [SerializeField]
    private bool draw_tiles = true;

    [SerializeField] [HideInInspector]
    List<GridTile> tiles = new List<GridTile>();

    [System.Serializable]
    public class GridTile
    {
        public Vector2 pos = Vector2.zero;
        public Vector2Int grid_pos = Vector2Int.zero;
        public GameObject go = null;
    }

    private void Update()
    {
        SetGridCenter(transform.position);

        UpdateGrid();

        if(draw_tiles)
            DebugDrawGrid();
    }

    public void CreateGrid(Vector2Int size)
    {
        grid_size = size;
    }

    public Vector2Int GetGridSize()
    {
        return grid_size;
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

    public List<GridTile> GetTiles()
    {
        return tiles;
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

                if (curr_tile.go != null)
                {
                    curr_tile.go.transform.position = curr_tile.pos;
                    curr_tile.go.transform.localScale = new Vector3(tiles_size, tiles_size, tiles_size);
                    curr_tile.go.transform.parent = this.gameObject.transform;
                }
            }
        }
        else
        {
            Vector2Int curr_grid_size = grid_size;

            ClearGrid();

            grid_size = curr_grid_size;

            for (int y = 0; y < grid_size.y; ++y)
            {
                for (int x = 0; x < grid_size.x; ++x)
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
                DestroyImmediate(curr_tile.go);
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
