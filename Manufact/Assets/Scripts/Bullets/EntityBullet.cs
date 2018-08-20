using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBullet : MonoBehaviour
{
    private float movement_speed = 0.0f;
    private EntityPathInstance.PathPointDirection movement_dir;
    private GameEntity sender = null;

    private BoxCollider2D collider = null;

    private Timer destruction_timer = new Timer();

    private void Awake()
    {
        destruction_timer.Start();

        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        UpdateMovement();

        if (destruction_timer.ReadTime() > 4.0f)
            Destroy(gameObject);
    }

    public void Init(GameEntity entity_sender, float speed, EntityPathInstance.PathPointDirection direction)
    {
        entity_sender = sender;
        movement_speed = speed;
        movement_dir = direction;
    }

    private void UpdateMovement()
    {
        Vector3 move_val = Vector3.zero;
        float speed_dt = movement_speed * Time.deltaTime;
        Quaternion rotation = Quaternion.identity;

        switch(movement_dir)
        {
            case EntityPathInstance.PathPointDirection.PATH_POINT_DIRECTION_UP:
                move_val = new Vector3(0, speed_dt, transform.position.z);
                rotation = Quaternion.Euler(0, 0, 0);
                break;
            case EntityPathInstance.PathPointDirection.PATH_POINT_DIRECTION_DOWN:
                move_val = new Vector3(0, -speed_dt, transform.position.z);
                rotation = Quaternion.Euler(0, 0, 180);
                break;
            case EntityPathInstance.PathPointDirection.PATH_POINT_DIRECTION_LEFT:
                move_val = new Vector3(-speed_dt, 0, transform.position.z);
                rotation = Quaternion.Euler(0, 0, 90);
                break;
            case EntityPathInstance.PathPointDirection.PATH_POINT_DIRECTION_RIGHT:
                move_val = new Vector3(speed_dt, 0, transform.position.z);
                rotation = Quaternion.Euler(0, 0, 270);
                break;
        }

        transform.position += move_val;
        transform.rotation = rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameEntity entity_hit = collision.gameObject.GetComponent<GameEntity>();

        if (entity_hit != null)
        {
            if(sender != entity_hit)
            {
                EventManager.Event ev = new EventManager.Event(EventManager.EventType.ENTITY_HIT);
                ev.entity_hit.hit = entity_hit;
                ev.entity_hit.sender = sender;
                EventManager.Instance.SendEvent(ev);

                Destroy(gameObject);
                return;
            }
        }

        GridTileInstance tile_hit = collision.gameObject.GetComponent<GridTileInstance>();

        if(tile_hit != null)
        {
            EventManager.Event ev = new EventManager.Event(EventManager.EventType.TILE_HIT);
            ev.tile_hit.tile = tile_hit;
            ev.tile_hit.sender = sender;
            EventManager.Instance.SendEvent(ev);

            Destroy(gameObject);
            return;
        }
    }
}
