using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : Level
{
    [Header("Level info")]
    [SerializeField] private int player_bullets;
    [SerializeField] private int enemy_lifes;

    private int curr_player_bullets = 0;
    private int curr_enemy_lifes = 0;

    private EntityPlayer player = null;
    private EntityBaseEnemy enemy = null;

    private bool enemy_dead = false;
    private bool no_bullets = false;

    public override void OnAwake()
    {
        EventManager.Instance.Suscribe(OnEvent);
    }

    public override void OnEnd()
    {
        EventManager.Instance.UnSuscribe(OnEvent);
    }

    public override void OnStart()
    {
        curr_player_bullets = player_bullets;
        curr_enemy_lifes = enemy_lifes;
        enemy_dead = false;

        path.ReloadPath();
        grid.ReloadGrid();

        player = (EntityPlayer)path.GetGameEntityByEntityType(EntityPathInstance.PathEntityType.PATH_ENTITY_TYPE_PLAYER);
        enemy = (EntityBaseEnemy)path.GetGameEntityByEntityType(EntityPathInstance.PathEntityType.PATH_ENTITY_TYPE_BASE_ENEMY);

        if(player != null)
        {
            player.SetBullets(player_bullets);
        }

        if(enemy != null)
        {
            enemy.SetLifePoints(enemy_lifes);
        }
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
        return no_bullets;
    }

    private void OnEvent(EventManager.Event ev)
    {
        switch(ev.Type())
        {
            case EventManager.EventType.ENTITY_DIES:
                if(ev.entity_dies.entity == enemy)
                {
                    enemy_dead = true;
                }
                break;
            case EventManager.EventType.ENTITY_SHOOT_FINISHED:
                if(ev.entity_shoot_finished.sender == player)
                {
                    if(player.GetBullets() <= 0)
                    {
                        no_bullets = true;
                    }
                }
                break;
        }
    }
}
