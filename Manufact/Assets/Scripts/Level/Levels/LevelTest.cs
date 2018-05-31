using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTest : Level
{
    [Header("Custom level info")]
    [SerializeField] private GameObject grid = null;
    private GridInstance ins = null;

    public override void OnAwake()
    {
        if(grid != null)
        {
            ins = grid.GetComponent<GridInstance>();

            if(ins != null)
            {
                MapManager.Instance.SetCurrGrid(ins);
            }
        }
    }

    public override void OnStart()
    {

    }

    public override void OnEnd()
    {

    }

    public override void OnUpdate()
    {

    }

    public override bool OnCheckWin()
    {
        return false;
    }

    public override bool OnCheckLose()
    {
        return false;
    }
}
