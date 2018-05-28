using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlotInstance : MonoBehaviour
{
    [SerializeField] private GridInstance.SlotMode slot_mode = GridInstance.SlotMode.INTERACTABLE;

    private GridInstance.Grid grid = null;
    private GridInstance.GridSlot slot = null;

    public void SetInfo(GridInstance.Grid _grid, GridInstance.GridSlot _slot)
    {
        grid = _grid;
        slot = _slot;
    }

    public GridInstance.SlotMode GetSlotMode()
    {
        return slot_mode;
    }

    private void OnMouseDown()
    {
        if (grid != null && slot != null)
            grid.SlotMouseDown(slot);
    }

    private void OnMouseEnter()
    {
        if (grid != null && slot != null)
            grid.SlotMouseEnter(slot);
    }

    private void OnMouseOver()
    {
        if (grid != null && slot != null)
            grid.SlotMouseOver(slot);
    }

    private void OnMouseDrag()
    {
        if (grid != null && slot != null)
            grid.SlotMouseDrag(slot);
    }

    private void OnMouseExit()
    {
        if (grid != null && slot != null)
            grid.SlotMouseExit(slot);
    }

    private void OnMouseUp()
    {
        if (grid != null && slot != null)
            grid.SlotMouseUp(slot);
    }
}
