using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInstance : MonoBehaviour
{
    public enum SlotMode
    {
        INTERACTABLE,
        NO_INTERACTABLE,
    }

    public class Grid
    {
        public Grid(List<GridInstanceEditor.GameObjectGrid> _grid_slots, float _slots_size)
        {
            for (int i = 0; i < _grid_slots.Count; ++i)
            {
                GridInstanceEditor.GameObjectGrid curr_gridgo = _grid_slots[i];

                if (curr_gridgo != null)
                {
                    SpriteRenderer sr = curr_gridgo.go.AddComponent<SpriteRenderer>();

                    BoxCollider coll = curr_gridgo.go.AddComponent<BoxCollider>();
                    coll.size = new Vector3(1, 1, 0.1f);
                    coll.isTrigger = true;

                    GridSlot slot = new GridSlot(curr_gridgo.go, sr, coll, curr_gridgo.pos);

                    grid_slots.Add(slot);
                }
            }

            slots_size = _slots_size;
        }

        GridSlot GetGridSlot(Vector2Int grid_pos)
        {
            GridSlot ret = null;

            for (int i = 0; i < grid_slots.Count; ++i)
            {
                GridSlot curr_slot = grid_slots[i];

                if (curr_slot.GetGridPos() == grid_pos)
                {
                    ret = curr_slot;
                    break;
                }
            }

            return ret;
        }

        private List<GridSlot> grid_slots = new List<GridSlot>();
        private float slots_size = 0.0f;
    }

    public class GridSlot
    {
        public GridSlot(GameObject _game_object, SpriteRenderer _sprite_renderer, BoxCollider _collider, Vector2Int grid_pos)
        {
            game_object = _game_object;
            sprite_renderer = _sprite_renderer;
            collider = _collider;
        }

        public GameObject GetGameObject() { return game_object; }
        public SpriteRenderer GetSpriteRenderer() { return sprite_renderer; }
        public BoxCollider GetCollider() { return collider; }
        public Vector2Int GetGridPos() { return grid_pos; }

        private GameObject game_object = null;
        private SpriteRenderer sprite_renderer = null;
        private BoxCollider collider = null;
        private Vector2Int grid_pos = Vector2Int.zero;
        private SlotMode mode = SlotMode.INTERACTABLE;
    }

    private Grid grid = null;

    public void SetGridInfo(List<GridInstanceEditor.GameObjectGrid> gos, float slots_size)
    {      
       grid = new Grid(gos, slots_size);  
    }

    public Grid GetGrid()
    {
        return grid;
    }
}
