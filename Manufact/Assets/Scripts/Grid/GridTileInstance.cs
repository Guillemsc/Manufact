using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridTileInstance : MonoBehaviour
{
    private enum GridTileAnimation
    {
        NO_ANIMATION,
        SCALE_ROTATE_DISAPPEAR,
    }

    private GameGridInstance grid = null;

    private Collider2D collider = null;

    private GridTileAnimation curr_animation = GridTileAnimation.NO_ANIMATION;
    private Transform start_animation_transform = null;

    GameGridInstance.GridTileType type = new GameGridInstance.GridTileType(); 

    public void Init(GameGridInstance grid_in, GameGridInstance.GridTileType grid_type)
    {
        grid = grid_in;
        type = grid_type;

        collider = gameObject.GetComponent<Collider2D>();
    }

	private void Start ()
    {
        EventManager.Instance.Suscribe(OnEvent);
	}

    private void OnDestroy()
    {
        if (EventManager.Valid())
            EventManager.Instance.UnSuscribe(OnEvent);
    }

    public GameGridInstance.GridTileType Type()
    {
        return type;
    }

    private bool BulletCanDestroyTile(EntityBullet.EntityBulletType bullet_type, GameGridInstance.GridTileType tile_type)
    {
        bool ret = false;

        if (bullet_type == EntityBullet.EntityBulletType.HIT_MOVE_TILE && tile_type == GameGridInstance.GridTileType.GRID_TILE_TYPE_MOVE)
            ret = true;

        if (bullet_type == EntityBullet.EntityBulletType.HIT_STATIC_TILE && tile_type == GameGridInstance.GridTileType.GRID_TIILE_TYPE_STATIC)
            ret = true;

        return ret;
    }

    private void StartAnimation(GridTileAnimation ani)
    {
        if (curr_animation == GridTileAnimation.NO_ANIMATION)
        {
            curr_animation = ani;

            switch (ani)
            {
                case GridTileAnimation.SCALE_ROTATE_DISAPPEAR:
                    {
                        start_animation_transform = gameObject.transform;
                        gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.4f);
                        gameObject.transform.DORotate(new Vector3(0, 0, 900), 0.4f);
                    }
                    break;
            }
        }
    }

    private void OnEvent(EventManager.Event ev)
    {
        switch (ev.Type())
        {
            case EventManager.EventType.TILE_HIT:
                if (ev.tile_hit.tile == this)
                {
                    if (BulletCanDestroyTile(ev.tile_hit.bullet.Type(), type))
                    {
                        grid.RemoveGameObject(gameObject);

                        collider.enabled = false;
                        StartAnimation(GridTileAnimation.SCALE_ROTATE_DISAPPEAR);
                    }
                }
                break;
        }
    }
}
