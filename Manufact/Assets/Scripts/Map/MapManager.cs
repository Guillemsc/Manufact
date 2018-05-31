using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private GameObject belt = null;

    private GridInstance.Grid curr_grid = null;

    List<MapEntity> map_entities = new List<MapEntity>();

    private void Awake()
    {
        InitInstance(this);
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

    public MapEntity SpawnEntity(Vector2Int grid_pos, MapEntity.MapEntityType type)
    {
        MapEntity ret = null;
        GridEntity grid_ent = null;
        GameObject to_spawn = null;

        CheckGridNull();
        if (curr_grid != null)
        {
            switch(type)
            {
                case MapEntity.MapEntityType.ENTITY_BELT:
                    {
                        if (belt != null)
                        {
                            to_spawn = belt;
                        }
                        break;
                    }
            }

            if (to_spawn != null)
            {
                grid_ent = curr_grid.InstantiateGridEntity(grid_pos, to_spawn);

                ret = grid_ent.GetComponent<MapEntity>();

                if (ret != null)
                {
                    ret.SetGridEntity(grid_ent);

                    ret.OnSpawn();

                    EventManager.Event ev = new EventManager.Event(EventManager.EventType.MAP_ENTITY_SPAWN);
                    ev.map_entity_spawn.entity = ret;
                    EventManager.Instance.SendEvent(ev);

                    map_entities.Add(ret);
                }
                else
                {
                    curr_grid.RemoveGridEntity(grid_ent);
                }
            }
        }

        return ret;
    }

    public void DeleteEntity(MapEntity entity)
    {
        if (entity != null)
        {
            CheckGridNull();
            if (curr_grid != null)
            {
                EventManager.Event ev = new EventManager.Event(EventManager.EventType.MAP_ENTITY_DELETE);
                ev.map_entity_delete.entity = entity;
                EventManager.Instance.SendEvent(ev);

                entity.OnDelete();

                curr_grid.RemoveGridEntity(entity.gameObject);
                map_entities.Remove(entity);

                Destroy(entity.gameObject);
            }
        }
    }

    public void ClearMapEntities()
    {
        CheckGridNull();
        while (map_entities.Count > 0)
        {
            DeleteEntity(map_entities[0]);
        }
    }

    private void OnSlotMouseDown(GridInstance.GridSlot slot)
    {
        if(curr_grid != null)
        {
            curr_grid.InstantiateGridEntity(slot.GetGridPos(), belt);
        }
    }

    private void CheckGridNull()
    {
        if (curr_grid == null)
            Debug.LogError("[Grid] The current grid on the MapManager is null, use SetCurrGrid");
    }
}
