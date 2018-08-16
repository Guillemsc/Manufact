using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : Level
{
    [Header("Level info")]
    [SerializeField] private int player_bullets;
    [SerializeField] private int enemy_lifes;

    private EntityPlayer player = null;
    private EntityBaseEnemy enemy = null;

    private bool enemy_dead = false;

    public override void OnAwake()
    {
        EventManager.Instance.Suscribe(OnEvent);
    }

    public override void OnStart()
    {
        player = (EntityPlayer)path.GetGameEntityByEntityType(EntityPathInstance.PathEntityType.PATH_ENTITY_TYPE_PLAYER);
        enemy = (EntityBaseEnemy)path.GetGameEntityByEntityType(EntityPathInstance.PathEntityType.PATH_ENTITY_TYPE_BASE_ENEMY);

        if(player != null)
        {
            player.SetBullets(1);
        }
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
        return enemy_dead;
    }

    public override bool OnCheckLose()
    {
        return false;
    }

    private void OnEvent(EventManager.Event ev)
    {

    }
}
