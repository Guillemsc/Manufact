using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBullet : MonoBehaviour
{
    public enum EntityBulletType
    {
        HIT_MOVE_TILE,
        HIT_STATIC_TILE,
    }

    private float movement_speed = 0.0f;
    private Vector2 direction_norm = Vector2.zero;
    private float rotation_angle = 0.0f;
    private GameEntity sender = null;
    private EntityBulletType type;

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

    public void Init(GameEntity entity_sender, float speed, EntityBulletType bullet_type)
    {
        sender = entity_sender;
        movement_speed = speed;
        direction_norm = entity_sender.gameObject.transform.up.normalized;
        rotation_angle = entity_sender.gameObject.transform.eulerAngles.z;
        type = bullet_type;
    }

    public EntityBulletType Type()
    {
        return type;
    }

    private void UpdateMovement()
    {
        Vector3 move_val = Vector3.zero;
        float speed_dt = movement_speed * Time.deltaTime;
        Quaternion rotation = Quaternion.identity;

        move_val = direction_norm * speed_dt;
        rotation = Quaternion.Euler(0, 0, rotation_angle);

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
            ev.tile_hit.bullet = this;
            EventManager.Instance.SendEvent(ev);

            Destroy(gameObject);

            return;
        }
    }

    private void OnDestroy()
    {
        EventManager.Event ev = new EventManager.Event(EventManager.EventType.ENTITY_SHOOT_FINISHED);
        ev.entity_shoot_finished.sender = sender;
        EventManager.Instance.SendEvent(ev);
    }
}
