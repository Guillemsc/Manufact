using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private GameObject belt = null;

    private GridInstance.Grid curr_grid = null;

    private void Awake()
    {
        InitInstance(this);
    }

    private void Start()
    {

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

    private MapEntity SpawnEntity(Vector2Int grid_pos, MapEntity.MapEntityType type)
    {
        MapEntity ret = null;

        if (curr_grid != null)
        {
            switch(type)
            {
                case MapEntity.MapEntityType.ENTITY_BELT:
                    {
                        if (belt != null)
                        {
                            GridEntity ins = curr_grid.InstantiateGridEntity(grid_pos, belt);

                            if (ins != null)
                            {
                                ret = ins.GetComponent<MapEntity>();

                                if (ret == null)
                                {
                                    curr_grid.RemoveGridEntity(ins);
                                }
                            }
                        }
                        break;
                    }
            }
        }

        if(ret != null)
        {
            EventManager.Event ev = new EventManager.Event(EventManager.EventType.MAP_ENTITY_SPAWN);
            ev.map_entity_spawn.entity = ret;
            EventManager.Instance.SendEvent(ev);
        }

        return ret;
    }

    private void DeleteEntity(MapEntity entity)
    {
        if (entity != null)
        {
            if (curr_grid != null)
            {
                EventManager.Event ev = new EventManager.Event(EventManager.EventType.MAP_ENTITY_DELETE);
                ev.map_entity_delete.entity = entity;
                EventManager.Instance.SendEvent(ev);

                curr_grid.RemoveGridEntity(entity.gameObject);

                Destroy(entity.gameObject);
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
