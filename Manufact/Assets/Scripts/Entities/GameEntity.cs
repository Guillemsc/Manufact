using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class GameEntity : MonoBehaviour
{
    private enum GameEntityAnimation
    {
        NO_ANIMATION,
        SCALE_ROTATE_DISAPPEAR,
    }

    protected EntityPathInstance path = null;

    protected EntityPathInstance.PathEntityType type;

    protected int life_points = 1;
    protected List<EntityBullet.EntityBulletType> bullets = new List<EntityBullet.EntityBulletType>();
    private List<GameObject> visual_bullets = new List<GameObject>();

    protected bool dead = false;

    private Timer timer_before_new_shoot = new Timer();
    private float time_before_new_shoot = 1.0f;

    private Collider2D collider = null;

    private GameEntityAnimation curr_animation = GameEntityAnimation.NO_ANIMATION;
    private Transform start_animation_transform = null;

    public void Awake()
    {
        timer_before_new_shoot.Start();

        EventManager.Instance.Suscribe(OnEvent);

        collider = gameObject.GetComponent<Collider2D>();
    }

    public void OnDestroy()
    {
        if(EventManager.Valid())
            EventManager.Instance.UnSuscribe(OnEvent);
    }

    public void Init(EntityPathInstance ins)
    {
        path = ins;
    }

    public void SetBullets(List<EntityBullet.EntityBulletType> bullets_to_add)
    {
        if (bullets != null)
        {
            bullets = new List<EntityBullet.EntityBulletType>(bullets_to_add);

            SetVisualBullets();
        }
    }

    public EntityBullet.EntityBulletType GetNextBullet()
    {
        EntityBullet.EntityBulletType ret = new EntityBullet.EntityBulletType();

        if (bullets.Count > 0)
            ret = bullets[0];

        return ret;
    }

    public void RemoveNextBullet()
    {
        if (bullets.Count > 0)
            bullets.RemoveAt(0);

        SetVisualBullets();
    }

    public int GetBulletsCount()
    {
        return bullets.Count;
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

            path.RemoveGameObject(gameObject);
            collider.enabled = false;
            StartAnimation(GameEntityAnimation.SCALE_ROTATE_DISAPPEAR);
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
            if (path != null && GetBulletsCount() > 0)
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

                EntityBullet.EntityBulletType type = GetNextBullet();

                GameObject bullet = InstantiateBulletGoFromBulletType(type);
                EntityBullet bullet_script = bullet.AddComponent<EntityBullet>();
                bullet_script.Init(this, LevelCreatorEditor.Instance.GetBulletsSpeed(), type, point.direction);

                RemoveNextBullet();

                timer_before_new_shoot.Start();

                EventManager.Event ev = new EventManager.Event(EventManager.EventType.ENTITY_SHOOTS);
                ev.entity_shoots.entity = this;
                EventManager.Instance.SendEvent(ev);

                ret = true;
            }
        }

        return ret;
    }

    GameObject InstantiateBulletGoFromBulletType(EntityBullet.EntityBulletType type)
    {
        GameObject ret = null;

        GameObject prefab = null;
        switch (type)
        {
            case EntityBullet.EntityBulletType.HIT_MOVE_TILE:
                prefab = LevelCreatorEditor.Instance.GetBulletHitMove();
                break;
            case EntityBullet.EntityBulletType.HIT_STATIC_TILE:
                prefab = LevelCreatorEditor.Instance.GetBulletHitStatic();
                break;
        }

        if (prefab != null)
        {
            ret = Instantiate(prefab, transform.position, Quaternion.identity);
            ret.transform.parent = transform;
        }

        return ret;
    }

    GameObject InstantiateVisualBulletGoFromBulletType(EntityBullet.EntityBulletType type)
    {
        GameObject ret = null;

        GameObject prefab = null;
        switch (type)
        {
            case EntityBullet.EntityBulletType.HIT_MOVE_TILE:
                prefab = LevelCreatorEditor.Instance.GetBulletAmmoHitMove();
                break;
            case EntityBullet.EntityBulletType.HIT_STATIC_TILE:
                prefab = LevelCreatorEditor.Instance.GetBulletAmmoHitStatic();
                break;
        }

        if (prefab != null)
        {
            ret = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            ret.transform.parent = transform;
        }

        return ret;
    }


    private void SetVisualBullets()
    {
        if (bullets != null)
        {
            for (int i = 0; i < visual_bullets.Count; ++i)
            {
                Destroy(visual_bullets[i]);
            }

            visual_bullets.Clear();

            for (int i = 0; i < bullets.Count; ++i)
            {
                GameObject new_bull = InstantiateVisualBulletGoFromBulletType(bullets[i]);

                float start_pos = 0.86f;

                float to_add = 0.15f;

                if (i == 0)
                    to_add = 0.46f;

                new_bull.transform.parent = this.transform;
                new_bull.transform.localScale = new Vector3(1, 1, 1);
                new_bull.transform.localPosition = new Vector3((start_pos + (i * to_add)),
                    -0.47f, 0);

                visual_bullets.Add(new_bull);
            }
        }
    }

    private void StartAnimation(GameEntityAnimation ani)
    {
        if(curr_animation == GameEntityAnimation.NO_ANIMATION)
        {
            curr_animation = ani;

            switch(ani)
            {
                case GameEntityAnimation.SCALE_ROTATE_DISAPPEAR:
                    {
                        start_animation_transform = gameObject.transform;
                        gameObject.transform.DOScale(new Vector3(0, 0, 0), 1.0f);
                    }
                    break;
            }
        }
    }

    private void OnEvent(EventManager.Event ev)
    {
        OnEventCall(ev);

        switch (ev.Type())
        {
            case EventManager.EventType.ENTITY_HIT:
                if(ev.entity_hit.hit == this)
                {
                    SetLifeHit(1);
                }
                break;
        }
    }

    protected abstract void OnEventCall(EventManager.Event ev);
}
