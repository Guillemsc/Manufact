using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLevel : Level
{
    [Header("Level info")]
    [SerializeField] private GameObject level_background_prefab = null;

    [SerializeField] private List<EntityBullet.EntityBulletType> bullets = new List<EntityBullet.EntityBulletType>();
    [SerializeField] private int enemy_lifes;

    private int curr_enemy_lifes = 0;

    private EntityPlayer player = null;
    private EntityBaseEnemy enemy = null;

    private bool enemy_dead = false;
    private bool no_bullets = false;

    public override void OnAwake()
    {
        EventManager.Instance.Suscribe(OnEvent);

        GameObject background = Instantiate(level_background_prefab, new Vector3(0, 0, 0), Quaternion.identity);

        background.transform.parent = gameObject.transform;
    }

    public override void OnEnd()
    {
        EventManager.Instance.UnSuscribe(OnEvent);
    }

    public override void OnStart()
    {
        curr_enemy_lifes = enemy_lifes;
        enemy_dead = false;
        no_bullets = false;

        path.ReloadPath();
        grid.ReloadGrid();

        player = (EntityPlayer)path.GetGameEntityByEntityType(EntityPathInstance.PathEntityType.PATH_ENTITY_TYPE_PLAYER);
        enemy = (EntityBaseEnemy)path.GetGameEntityByEntityType(EntityPathInstance.PathEntityType.PATH_ENTITY_TYPE_BASE_ENEMY);

        if(player != null)
        {
            player.SetBullets(bullets);
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

    public List<EntityBullet.EntityBulletType> GetBulletsList()
    {
        return bullets;
    }

    public void SetBulletsNumber(int set)
    {
        if (set != bullets.Count && set >= 0)
        {
            bullets.Clear();

            for(int i = 0; i < set; ++i)
            {
                bullets.Add(EntityBullet.EntityBulletType.HIT_MOVE_TILE);
            }
        }
    }

    public void SetBulletType(int index, EntityBullet.EntityBulletType type)
    {
        if (bullets.Count > index)
            bullets[index] = type;
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
                    if(player.GetBulletsCount() <= 0)
                    {
                        no_bullets = true;
                    }
                }
                break;
        }
    }
}
