using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileInstance : MonoBehaviour
{
    private GameGridInstance grid = null;

    public void Init(GameGridInstance grid_in)
    {
        grid = grid_in;
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

    private void OnEvent(EventManager.Event ev)
    {
        switch (ev.Type())
        {
            case EventManager.EventType.TILE_HIT:
                if (ev.tile_hit.tile == this)
                {
                    grid.RemoveGameObject(gameObject);

                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
