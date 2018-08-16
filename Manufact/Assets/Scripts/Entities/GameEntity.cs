using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    protected EntityPathInstance path = null;

    protected EntityPathInstance.PathEntityType type;

    protected int life_points = 0;
    protected int bullets_count = 0;

    protected bool dead = false;

    private Timer timer_before_new_shoot = new Timer();
    private float time_before_new_shoot = 1.0f;

    BoxCollider2D collider = null;

    public void Awake()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();

        timer_before_new_shoot.Start();
    }

    public void OnDestroy()
    {
      
    }

    public void Init(EntityPathInstance ins)
    {
        path = ins;
    }

    public void SetBullets(int set)
    {
        bullets_count = set;

        if (bullets_count < 0)
            bullets_count = 0;
    }

    public int GetBullets()
    {
        return bullets_count;
    }

    public void SetLifePoints(int set)
    {
        life_points = set;

        if (life_points < 0)
            life_points = 0;

        if(life_points == 0 && !dead)
        {
            dead = true;

            EventManager.Event ev = new EventManager.Event(EventManager.EventType.ENTITY_DIES);
            ev.entity_dies.entity = this;
            EventManager.Instance.SendEvent(ev);
        }
    }

    public void SetLifeHit(int hit)
    {
        SetLifePoints(life_points -= hit);
    }

    public int GetLifePoints()
    {
        return life_points;
    }

    public bool GetIsDead()
    {
        return life_points <= 0;
    }

    public bool Shoot()
    {
        bool ret = false;

        if (timer_before_new_shoot.ReadTime() > time_before_new_shoot)
        {
            if (path != null && bullets_count > 0)
            {
                EntityPathInstance.PathPoint point = path.GetPathPointFromEntityGo(gameObject);

                if (point != null)
                {
                    switch (point.direction)
                    {
                        case EntityPathInstance.PathPointDirection.PATH_POINT_DIRECTION_UP:
                            break;
                        case EntityPathInstance.PathPointDirection.PATH_POINT_DIRECTION_DOWN:
                            break;
                        case EntityPathInstance.PathPointDirection.PATH_POINT_DIRECTION_LEFT:
                            break;
                        case EntityPathInstance.PathPointDirection.PATH_POINT_DIRECTION_RIGHT:
                            break;
                    }
                }


                GameObject bullet = Instantiate(LevelCreatorEditor.Instance.GetBaseBullet(), transform.position, Quaternion.identity);
                EntityBullet bullet_script = bullet.AddComponent<EntityBullet>();
                bullet_script.Init(this, LevelCreatorEditor.Instance.GetBulletsSpeed(), point.direction);

                SetBullets(bullets_count - 1);

                timer_before_new_shoot.Start();

                EventManager.Event ev = new EventManager.Event(EventManager.EventType.ENTITY_SHOOTS);
                ev.entity_shoots.entity = this;
                EventManager.Instance.SendEvent(ev);

                ret = true;
            }
        }

        return ret;
    }
}
