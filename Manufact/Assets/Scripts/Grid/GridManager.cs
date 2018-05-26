using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            go.transform.parent = transform;
        }
        else if(grids_list.Count == 1)
        {
            if (grid != null)
            {
                grid.transform.parent = null;
                grid.gameObject.name = "GRID";
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
