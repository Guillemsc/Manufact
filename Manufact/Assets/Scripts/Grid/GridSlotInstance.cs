using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlotInstance : MonoBehaviour
{
    [SerializeField] private GridInstance.SlotMode slot_mode = GridInstance.SlotMode.INTERACTABLE;

    public GridInstance.SlotMode GetSlotMode()
    {
        return slot_mode;
    }
}
