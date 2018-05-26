using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapInstance : MonoBehaviour
{
    private Tilemap tile_map = null;

    private void Awake()
    {
        tile_map = gameObject.GetComponent<Tilemap>();
    }

    Vector3 GridPosToWorldPos(Vector2Int tile_map_pos)
    {
        Vector3 ret = Vector3.zero;

        if (tile_map != null)
            ret = tile_map.GetCellCenterWorld(new Vector3Int(tile_map_pos.x, tile_map_pos.y, 0));
        
        return ret;
    }

    Vector2Int WorldPosToGridPos(Vector3 world_pos)
    {
        Vector2Int ret = Vector2Int.zero;

        if (tile_map != null)
        {
            Vector3Int cell_pos = tile_map.WorldToCell(world_pos);

            ret = new Vector2Int(cell_pos.x, cell_pos.y);
        }

        return ret;
    }

    private void Update()
    {
        Debug.Log(GridPosToWorldPos(new Vector2Int(0, 0)));
    }
}
