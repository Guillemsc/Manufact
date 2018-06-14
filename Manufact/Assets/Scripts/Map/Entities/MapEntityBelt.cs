using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntityBelt : MapEntity
{
    [SerializeField]
    private List<GameObject> path_points = new List<GameObject>();

    private float move_speed = 0.0f;

    private List<MaterialEntity> moving_materials = new List<MaterialEntity>();

    private void Awake()
    {
        SetEntityType(MapEntityType.ENTITY_BELT);
        SetMapEntityDir(MapEntityDir.RIGHT);
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        LookForNextBelt();

    }

    public override void OnSpawn()
    {

    }

    public override void OnDelete()
    {
       
    }

    public List<GameObject> GetPathPoints()
    {
        return path_points;
    }

    public void SetMoveSpeed(float set)
    {
        move_speed = set;

        if (move_speed < 0)
            move_speed = 0.0f;
    }

    public float GetMoveSpeed()
    {
        return move_speed;
    }

    private void AddMovingMaterial(MaterialEntity mat)
    {
        if (mat != null)
        {
            if (!moving_materials.Contains(mat))
                moving_materials.Add(mat);
        }
    }

    private void RemoveMovingMaterial(MaterialEntity mat)
    {
        if(mat != null)
            moving_materials.Remove(mat);
    }

    private MapEntityBelt LookForNextBelt()
    {
        MapEntityBelt ret = null;

        MapEntity front_entity = GetFrontEntity();

        if(front_entity != null)
        {
            int i = 0;
            ++i;
        }

        return ret;
    }
}
