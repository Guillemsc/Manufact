using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntityBelt : MapEntity
{
    private void Awake()
    {
        SetEntityType(MapEntityType.ENTITY_BELT);
        SetMapEntityDir(MapEntityDir.LEFT);
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public override void OnSpawn()
    {
        GridEntity entity = gameObject.GetComponent<GridEntity>();

        if (entity != null)
        {
            SetGridEntity(entity);
        }
    }

    public override void OnDelete()
    {
       
    }
}
