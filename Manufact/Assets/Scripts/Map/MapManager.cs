using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public enum MapClickBehaviour
    {
        ADD,
        ERASE,
    }

    [SerializeField] private GameObject belt = null;

    private GridInstance.Grid curr_grid = null;

    MapClickBehaviour map_click_behaviour = MapClickBehaviour.ADD;

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

    public void SetClickBehaviour(MapClickBehaviour set)
    {
        map_click_behaviour = set;
    }

    public MapClickBehaviour GetClickBehaviour()
    {
        return map_click_behaviour;
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

    public MapEntity GetEntityByMapPos(Vector2Int map_pos)
    {
        MapEntity ret = null;

        if (curr_grid != null)
        {
            for(int i = 0; i < map_entities.Count; ++i)
            {
                MapEntity curr_entity = map_entities[i];

                if(curr_entity.GetGridEntity().GetGridPos() == map_pos)
                {
                    ret = curr_entity;
                    break;
                }
            }
        }

        return ret;
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
            switch(map_click_behaviour)
            {
                case MapClickBehaviour.ADD:
                    SpawnEntity(slot.GetGridPos(), MapEntity.MapEntityType.ENTITY_BELT);
                    break;
                case MapClickBehaviour.ERASE:
                    break;
            }
        }
    }

    private void CheckGridNull()
    {
        if (curr_grid == null)
            Debug.LogError("[Grid] The current grid on the MapManager is null, use SetCurrGrid");
    }
}
