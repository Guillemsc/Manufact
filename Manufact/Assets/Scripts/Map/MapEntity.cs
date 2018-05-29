using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntity : MonoBehaviour
{
    public enum MapEntityType
    {
        ENTITY_BELT,
    }

    private MapEntityType type;

    public void SetEntityType(MapEntityType _type)
    {
        type = _type;
    }

    public MapEntityType GetEntityType()
    {
        return type;
    }

    public virtual void OnSpawn()
    {
    }

    public virtual void OnDelete()
    {
    }

    
}
