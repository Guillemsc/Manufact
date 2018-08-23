using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public enum EventType
    {
        EVENT_NULL,

        MAP_ENTITY_SPAWN,
        MAP_ENTITY_DELETE,

        LEVEL_STARTED,
        LEVEL_LOAD,
        LEVEL_UNLOAD,
        LEVEL_FINISHED,

        ENTITY_SHOOTS,
        ENTITY_HIT,
        ENTITY_DIES,
        ENTITY_BULLETS_CHANGE,

        CONTROLS_SWIPE_UP,
        CONTROLS_SWIPE_RIGHT,
        CONTROLS_SWIPE_DOWN,
        CONTROLS_SWIPE_LEFT,

        TILE_HIT,
    }

    public class Event
    {
        public Event(EventManager.EventType e_type)
        {
            event_type = e_type;
        }

        // Controls
        public class ControlsSwipeUp
        {      
        }
        public ControlsSwipeUp controls_swipe_up = new ControlsSwipeUp();

        public class ControlsSwipeLeft
        {
        }
        public ControlsSwipeLeft controls_swipe_left = new ControlsSwipeLeft();

        public class ControlsSwipeDown
        {
        }
        public ControlsSwipeDown controls_swipe_down = new ControlsSwipeDown();

        public class ControlsSwipeRight
        {
        }
        public ControlsSwipeRight controls_swipe_right = new ControlsSwipeRight();

        // Levels
        public class LevelStarted
        {
            public int level = 0;
        }
        public LevelStarted level_started = new LevelStarted();

        public class LevelLoad
        {
            public int level = 0;
        }
        public LevelLoad level_load = new LevelLoad();

        public class LevelFinished
        {
            public int level = 0;
        }
        public LevelFinished level_finished = new LevelFinished();

        public class LevelUnload
        {
            public int level = 0;
        }
        public LevelUnload level_unload = new LevelUnload();

        // Entities
        public class EntityShoots
        {
            public GameEntity entity = null;
        }
        public EntityShoots entity_shoots = new EntityShoots();

        public class EntityHit
        {
            public GameEntity hit = null;
            public GameEntity sender = null;
        }
        public EntityHit entity_hit = new EntityHit();

        public class EntityDies
        {
            public GameEntity entity = null;
        }
        public EntityDies entity_dies = new EntityDies();

        public class EntityBulletsChange
        {
            public GameEntity entity = null;
            public int bullets_now = 0;
            public int bullets_before = 0;
        }
        public EntityBulletsChange entity_bullets_change = new EntityBulletsChange();

        // Tiles
        public class TileHit
        {
            public GridTileInstance tile = null;
            public GameEntity sender = null;
        }
        public TileHit tile_hit = new TileHit();

        public EventManager.EventType Type()
        {
            return event_type;
        }

        private EventManager.EventType event_type = EventManager.EventType.EVENT_NULL;
    }

    private void Awake()
    {
        InitInstance(this, gameObject);
    }

    public void SendEvent(Event ev)
    {
        if (ev != null)
        {
            if (OnEvent != null)
                OnEvent(ev);
        }
    }

    public void Suscribe(OnEventDel del)
    {
        bool found = false;

        if (OnEvent != null)
        {
            foreach (Delegate d in OnEvent.GetInvocationList())
            {
                if ((OnEventDel)d == del)
                {
                    found = true;
                    break;
                }
            }
        }
    
        if(!found)
            OnEvent += del;
    }

    public void UnSuscribe(OnEventDel del)
    {
        OnEvent -= del;
    }

    public delegate void OnEventDel(EventManager.Event ev);
    private event OnEventDel OnEvent = null;
}
