using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : GameEntity
{

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKey("q"))
        {
            Shoot();
        }
    }

    protected override void OnEventCall(EventManager.Event ev)
    {

    }

    void OnMouseDown()
    {
        Shoot();
    }
}
