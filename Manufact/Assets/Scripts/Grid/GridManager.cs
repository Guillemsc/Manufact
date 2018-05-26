using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridManager : Singleton<GridManager>
{
    [SerializeField] [HideInInspector] private Grid grid = null;

    private void Awake()
    {
        InitInstance(this, gameObject);

        CheckGridInstance();
    }

    private void Update()
    {
        CheckGridInstance();
    }

    public Grid GetGrid()
    {
        return grid;
    }

    public void CreateTileMap()
    {
        if(grid != null)
        {
            GameObject tile_map_go = new GameObject();
            tile_map_go.name = "TileMap";

            Tilemap tile_map = tile_map_go.AddComponent<Tilemap>();

            TilemapRenderer renderer = tile_map_go.AddComponent<TilemapRenderer>();
            renderer.sortOrder = TilemapRenderer.SortOrder.BottomRight;

            tile_map_go.transform.parent = grid.transform;
        }
    }

    private void CheckGridInstance()
    {
        Grid[] grids = FindObjectsOfType<Grid>();

        List<Grid> grids_list = new List<Grid>();
        grids_list.AddRange(grids);

        if(grids_list.Count == 0)
        {
            GameObject go = new GameObject();
            go.name = "GRID";
            grid = go.AddComponent<Grid>();
            go.transform.parent = null;
        }
        else if(grids_list.Count == 1)
        {
            if (grid != null)
            {
                grid.transform.parent = null;
                grid.gameObject.name = "GRID";
                grid.transform.position = new Vector3(0, 0, 0);
                grid.transform.rotation = Quaternion.identity;
                grid.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if(grids_list.Count > 1)
        {
            while (grids_list.Count > 1)
            {
                DestroyImmediate(grids_list[grids_list.Count - 1].gameObject);
                grids_list.Remove(grids_list[grids_list.Count - 1]);
            }

            Debug.LogWarning("[Grid] Grid creation is restricted, if you want acces to the grid use the GridManager.Instance");
        }
    }

}
