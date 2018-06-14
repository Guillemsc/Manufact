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

    public Vector2Int GetGridPos()
    {
        Vector2Int ret = Vector2Int.zero;

        if (grid_slot != null)
            ret = grid_slot.GetGridPos();

        return ret;

    }

    public GridInstance.Grid GetGrid()
    {
        return grid;
    }

    public List<GridEntity> GetEntitiesUp()
    {
        List<GridEntity> ret = new List<GridEntity>();

        if(grid != null && grid_slot != null)
        {
            Vector2Int up_pos = grid_slot.GetGridPos();
            up_pos.x += 1;

            ret = grid.GetEntitiesOnGridPos(up_pos);
        }

        return ret;
    }

    public List<GridEntity> GetEntitiesDown()
    {
        List<GridEntity> ret = new List<GridEntity>();

        if (grid != null && grid_slot != null)
        {
            Vector2Int down_pos = grid_slot.GetGridPos();
            down_pos.x -= 1;

            ret = grid.GetEntitiesOnGridPos(down_pos);
        }

        return ret;
    }

    public List<GridEntity> GetEntitiesLeft()
    {
        List<GridEntity> ret = new List<GridEntity>();

        if (grid != null && grid_slot != null)
        {
            Vector2Int left_pos = grid_slot.GetGridPos();
            left_pos.y -= 1;

            ret = grid.GetEntitiesOnGridPos(left_pos);
        }

        return ret;
    }

    public List<GridEntity> GetEntitiesRight()
    {
        List<GridEntity> ret = new List<GridEntity>();

        if (grid != null && grid_slot != null)
        {
            Vector2Int right_pos = grid_slot.GetGridPos();
            right_pos.x += 1;

            ret = grid.GetEntitiesOnGridPos(right_pos);
        }

        return ret;
    }
}
