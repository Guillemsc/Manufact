using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : GameEntity
{
    //private void Awake()
    //{
    //    type = EntityPathInstance.PathEntityType.PATH_ENTITY_TYPE_PLAYER;
    //}

    private void Update()
    {
        if(Input.GetKey("q"))
        {
            Shoot();
        }
    }
}
