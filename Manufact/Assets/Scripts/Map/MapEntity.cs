using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntity : MonoBehaviour
{
    public enum MapEntityDir
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

    public enum MapEntityType
    {
        ENTITY_BELT,
    }

    private GridEntity grid_entity = null;

    private MapEntityDir direction = MapEntityDir.UP;
    private MapEntityType type;

    public void SetEntityType(MapEntityType _type)
    {
        type = _type;
    }

    public MapEntityType GetEntityType()
    {
        return type;
    }

    public void SetGridEntity(GridEntity _grid_entity)
    {
        grid_entity = _grid_entity;
    }

    public GridEntity GetGridEntity()
    {
        return grid_entity;
    }

    public void SetMapEntityDir(MapEntityDir dir)
    {       
        switch (dir)
        {
            case MapEntityDir.UP:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                direction = dir;
                break;
            case MapEntityDir.DOWN:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                direction = dir;
                break;
            case MapEntityDir.LEFT:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                direction = dir;
                break;
            case MapEntityDir.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                direction = dir;
                break;
        }
           

    }

    public MapEntityDir GetEntityDir()
    {
        return direction;
    }

    public virtual void OnSpawn()
    {
    }

    public virtual void OnDelete()
    {
    }

    
}
