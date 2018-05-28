using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    private GridInstance.Grid grid = null;
    private GridInstance.GridSlot grid_slot = null;

    public void SetInfo(GridInstance.Grid _grid, GridInstance.GridSlot _grid_slot)
    {
        grid = _grid;

        if(grid != null)
        {
            grid.AddGridEntity(this);
        }

        grid_slot = _grid_slot;
    }

    private void OnDestroy()
    {
        if (grid != null)
            grid.RemoveGridEntity(this);
    }

    public GridInstance.GridSlot GetGridSlot()
    {
        return grid_slot;
    }
}
