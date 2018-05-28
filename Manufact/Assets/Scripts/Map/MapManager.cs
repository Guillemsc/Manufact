using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private GameObject grid_test = null;
    [SerializeField] private GameObject belt = null;

    private GridInstance.Grid curr_grid = null;

    private void Awake()
    {
        InitInstance(this);
    }

    private void Start()
    {
        if (grid_test != null)
        {
            GridInstance g_ins = grid_test.GetComponent<GridInstance>();

            if(g_ins != null)
            {
                SetCurrGrid(g_ins);
            }
        }
    }

    public void SetCurrGrid(GridInstance grid_inst)
    {
        if(grid_inst != null)
        {
            curr_grid = grid_inst.GetGrid();

            if(curr_grid != null)
            {
                curr_grid.OnSlotMouseDown += OnSlotMouseDown;
            }
        }
    }

    private void OnSlotMouseDown(GridInstance.GridSlot slot)
    {
        if(curr_grid != null)
        {
            curr_grid.InstantiateGridEntity(slot.GetGridPos(), belt);
        }
    }
}
