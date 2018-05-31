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

    public enum SlotUsedBehaviour
    {
        SPAWN_ON_TOP,
        REPLACE,
        DONT_SPAWN,
    }

    public class Grid
    {
        public Grid(List<GridInstanceEditor.GameObjectGrid> _grid_slots, Vector2Int _grid_size, float _slots_size)
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

                    GridSlot slot = new GridSlot(curr_gridgo.go, curr_gridgo.pos, sr, coll);

                    grid_slots.Add(slot);

                    GridSlotInstance slot_instance = curr_gridgo.go.GetComponent<GridSlotInstance>();
                    if (slot_instance != null)
                        slot_instance.SetInfo(this, slot);
                }
            }

            grid_size = _grid_size;
            slots_size = _slots_size;
        }

        public GridSlot GetGridSlot(Vector2Int grid_pos)
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

        public GridEntity InstantiateGridEntity(Vector2Int pos, GameObject go_prefab, SlotUsedBehaviour behaviour = SlotUsedBehaviour.DONT_SPAWN)
        {
            GridEntity ret = null;

            GridSlot slot = GetGridSlot(pos);

            if (slot != null && go_prefab != null)
            {
                GameObject ins = null;

                switch (behaviour)
                {
                    case SlotUsedBehaviour.DONT_SPAWN:
                        {
                            List<GridEntity> slot_entities = GetEntitiesOnGridPos(pos);

                            if(slot_entities.Count == 0)
                            {
                                ins = Instantiate(go_prefab, slot.GetWorldPosition(), Quaternion.identity);
                            }
                        }
                        break;
                    case SlotUsedBehaviour.REPLACE:
                        {
                            List<GridEntity> slot_entities = GetEntitiesOnGridPos(pos);

                            if (slot_entities.Count > 0)
                            {
                                for(int i = 0; i < slot_entities.Count; ++i)
                                {
                                    Destroy(slot_entities[i].gameObject);
                                }

                                slot_entities.Clear();
                            }

                            ins = Instantiate(go_prefab, slot.GetWorldPosition(), Quaternion.identity);
                        }
                        break;
                    case SlotUsedBehaviour.SPAWN_ON_TOP:
                        break;
                }

                if(ins != null)
                {
                    ret = ins.AddComponent<GridEntity>();

                    ret.SetInfo(this, slot);

                    AddGridEntity(ret);
                }
            }

            return ret;
        }

        public List<GridEntity> GetEntitiesOnGridPos(Vector2Int grid_pos)
        {
            List<GridEntity> ret = new List<GridEntity>();

            for(int i = 0; i < grid_entities.Count; ++i)
            {
                GridSlot curr_slot = grid_entities[i].GetGridSlot();

                if(curr_slot != null)
                {
                    if(grid_pos == curr_slot.GetGridPos())
                    {
                        ret.Add(grid_entities[i]);
                    }
                }
            }

            return ret;
        }

        public void AddGridEntity(GridEntity entity)
        {
            if(!grid_entities.Contains(entity))
                grid_entities.Add(entity);
        }

        public void RemoveGridEntity(GridEntity entity)
        {
            grid_entities.Remove(entity);
        }

        public void RemoveGridEntity(GameObject entity)
        {
            for(int i = 0; i < grid_entities.Count; ++i)
            {
                GridEntity curr_entity = grid_entities[i];

                if(curr_entity.gameObject == entity)
                {
                    grid_entities.Remove(curr_entity);
                    break;
                }
            }
        }

        public Vector2Int GetGridSize()
        {
            return grid_size;
        }

        public void SlotMouseDown(GridSlot slot)
        {
            if(OnSlotMouseDown != null)
                OnSlotMouseDown(slot);
        }

        public void SlotMouseEnter(GridSlot slot)
        {
            if (OnSlotMouseEnter != null)
                OnSlotMouseEnter(slot);
        }

        public void SlotMouseOver(GridSlot slot)
        {
            if (OnSlotMouseOver != null)
                OnSlotMouseOver(slot);
        }

        public void SlotMouseDrag(GridSlot slot)
        {
            if (OnSlotMouseDrag != null)
                OnSlotMouseDrag(slot);
        }

        public void SlotMouseExit(GridSlot slot)
        {
            if (OnSlotMouseExit != null)
                OnSlotMouseExit(slot);
        }

        public void SlotMouseUp(GridSlot slot)
        {
            if (OnSlotMouseUp != null)
               OnSlotMouseUp(slot);
        }

        private List<GridSlot> grid_slots = new List<GridSlot>();
        private Vector2Int grid_size = Vector2Int.zero;
        private float slots_size = 0.0f;

        private List<GridEntity> grid_entities = new List<GridEntity>();

        public delegate void OnSlotEvent(GridSlot slot);
        public event OnSlotEvent OnSlotMouseDown;
        public event OnSlotEvent OnSlotMouseEnter;
        public event OnSlotEvent OnSlotMouseOver;
        public event OnSlotEvent OnSlotMouseDrag;
        public event OnSlotEvent OnSlotMouseExit;
        public event OnSlotEvent OnSlotMouseUp;
    }

    public class GridSlot
    {
        public GridSlot(GameObject _game_object, Vector2Int _grid_pos, SpriteRenderer _sprite_renderer, BoxCollider _collider)
        {
            game_object = _game_object;
            grid_pos = _grid_pos;
            sprite_renderer = _sprite_renderer;
            collider = _collider;
        }

        public GameObject GetGameObject()
        {
            return game_object;
        }

        public Vector3 GetWorldPosition()
        {
            Vector3 ret = Vector3.zero;

            if(game_object != null)
                ret = game_object.transform.position;
            
            return ret;
        }
        public SpriteRenderer GetSpriteRenderer()
        {
            return sprite_renderer;
        }

        public BoxCollider GetCollider()
        {
            return collider;
        }

        public Vector2Int GetGridPos()
        {
            return grid_pos;
        }

        private GameObject game_object = null;
        private SpriteRenderer sprite_renderer = null;
        private BoxCollider collider = null;
        private Vector2Int grid_pos = Vector2Int.zero;
        private SlotMode mode = SlotMode.INTERACTABLE;
    }

    private Grid grid = null;

    public void SetGridInfo(List<GridInstanceEditor.GameObjectGrid> gos, Vector2Int grid_size, float slots_size)
    {      
       grid = new Grid(gos, grid_size, slots_size);  
    }

    public Grid GetGrid()
    {
        if (grid == null)
            Debug.LogWarning("[Grid] Trying to get a null grid (grids are created on Awake)");

        return grid;
    }
}
