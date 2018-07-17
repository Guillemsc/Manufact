using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameGridMovement : MonoBehaviour
{
    private enum MoveDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

    [SerializeField]
    private float movement_time = 0.5f;

    private GameGridInstance grid_inst = null;

    private bool moving = false;

    private class TileGameObjectToMove
    {
        public Vector3 target_pos = Vector3.zero;
        public GameGridInstance.GridTile tile = null;
        public GameObject tile_go = null;
        public Vector2Int new_tile_pos = Vector2Int.zero;
    }

    private List<TileGameObjectToMove> tile_gos_to_move = new List<TileGameObjectToMove>();
    private List<TileGameObjectToMove> tile_gos_to_update = new List<TileGameObjectToMove>();

    private void Awake()
    {
        grid_inst = gameObject.GetComponent<GameGridInstance>();
    }

    private void Start()
    {
 
    }

    void Update ()
    {
        if(Input.GetKey("a"))
            Move(MoveDirection.LEFT);
        if (Input.GetKey("d"))
            Move(MoveDirection.RIGHT);
        if (Input.GetKey("w"))
            Move(MoveDirection.UP);
        if (Input.GetKey("s"))
            Move(MoveDirection.DOWN);

        UpdateMovingTileGos();
    }

    private void Move(MoveDirection dir)
    {
        if (!moving)
        {
            List<GameGridInstance.GridTile> tiles = grid_inst.GetTiles();

            for (int i = 0; i < tiles.Count; ++i)
            {
                GameGridInstance.GridTile curr_tile = tiles[i];

                if (curr_tile.go != null)
                {
                    Vector3 target_pos = curr_tile.pos;

                    bool can_move = CheckMovementTile(curr_tile, dir);

                    if (can_move)
                    {
                        Vector2Int grid_pos_to_offset = GetOffsetByDirection(dir);

                        Vector2Int grid_pos_to_check = curr_tile.grid_pos + grid_pos_to_offset;

                        target_pos = grid_inst.GetWorldPosByTilePos(grid_pos_to_check);

                        TileGameObjectToMove go_to_move = new TileGameObjectToMove();
                        go_to_move.tile = curr_tile;
                        go_to_move.new_tile_pos = grid_pos_to_check;
                        go_to_move.target_pos = target_pos;
                        go_to_move.tile_go = curr_tile.go;

                        go_to_move.tile_go.transform.DOMove(go_to_move.target_pos, movement_time);

                        tile_gos_to_move.Add(go_to_move);

                        moving = true;
                    }
                }
            }

            if (moving)
                grid_inst.SetAutomaticGridSnap(false);
        }
        
    }

    private bool CheckMovementTile(GameGridInstance.GridTile tile, MoveDirection dir)
    {
        bool ret = false;

        if (tile != null)
        {
            Vector2Int grid_pos_to_offset = GetOffsetByDirection(dir);

            Vector2Int grid_pos_to_check = tile.grid_pos + grid_pos_to_offset;

            GameGridInstance.GetTileByGridPosState state;
            GameGridInstance.GridTile tile_check = grid_inst.GetTileByGridPos(grid_pos_to_check, out state);

            switch(state)
            {
                case GameGridInstance.GetTileByGridPosState.SUCCES:
                    {
                        if (tile_check.go != null)
                            ret = CheckMovementTile(tile_check, dir);
                        else
                            ret = true;
                    }
                    break;
                case GameGridInstance.GetTileByGridPosState.NO_TIILE_FOUND:
                    {
                        ret = true;
                    }
                    break;
            }
        }

        return ret;
    }

    private Vector2Int GetOffsetByDirection(MoveDirection dir)
    {
        Vector2Int ret = Vector2Int.zero;

        switch (dir)
        {
            case MoveDirection.LEFT:
                {
                    ret.x -= 1;
                    break;
                }
            case MoveDirection.RIGHT:
                {
                    ret.x += 1;
                    break;
                }
            case MoveDirection.UP:
                {
                    ret.y += 1;
                    break;
                }
            case MoveDirection.DOWN:
                {
                    ret.y -= 1;
                    break;
                }
        }

        return ret;
    }

    private void UpdateMovingTileGos()
    {
        if(moving)
        {
            for(int i = 0; i < tile_gos_to_move.Count;)
            {
                TileGameObjectToMove curr = tile_gos_to_move[i];

                Vector3 move_dir = curr.target_pos - curr.tile.go.transform.position;
                move_dir.Normalize();

                if (Mathf.Abs(Vector3.Distance(curr.tile.go.transform.position, curr.target_pos)) <= 0.1f)
                {
                    tile_gos_to_move.RemoveAt(i);
                    tile_gos_to_update.Add(curr);
                }
                else
                    ++i;
            }

            if(tile_gos_to_move.Count == 0)
            {
                UpdateGridInternalData();

                tile_gos_to_update.Clear();

                moving = false;

                grid_inst.SetAutomaticGridSnap(true);
            }
        }
    }

    private void UpdateGridInternalData()
    {
        // Set game object to it's new tile position
        for (int i = 0; i < tile_gos_to_update.Count; ++i)
        {
            TileGameObjectToMove curr = tile_gos_to_update[i];

            GameGridInstance.GridTile to_swap = grid_inst.GetTileByGridPos(curr.new_tile_pos);

            to_swap.go = curr.tile_go;
        }

        // Set old tile game object references to null
        for (int i = 0; i < tile_gos_to_update.Count; ++i)
        {
            TileGameObjectToMove curr = tile_gos_to_update[i];

            GameGridInstance.GridTile to_swap = grid_inst.GetTileByGridPos(curr.new_tile_pos);

            if (to_swap.go == curr.tile.go)
            {
                curr.tile.go = null;
            }
        }
    }
}
