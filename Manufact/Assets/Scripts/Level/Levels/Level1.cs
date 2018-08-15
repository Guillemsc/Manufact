using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : Level
{
    [Header("Level info")]
    [SerializeField] private int player_bullets;
    [SerializeField] private int enemy_lifes;

    public override void OnAwake()
    {
        EventManager.Instance.Suscribe(OnEvent);
    }

    public override void OnStart()
    {

    }

    public override void OnEnd()
    {
        EventManager.Instance.UnSuscribe(OnEvent);
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

    private void OnEvent(EventManager.Event ev)
    {

    }
}
