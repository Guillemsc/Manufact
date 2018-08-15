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
        LEVEL_FINISHED,

        ENTITY_SHOOTS,
        ENTITY_HIT,
        ENTITY_DIES,
    }

    public class Event
    {
        public Event(EventManager.EventType e_type)
        {
            event_type = e_type;
        }

        // Levels
        public class LevelStarted
        {
            public int level = 0;
        }
        public LevelStarted level_started = new LevelStarted();

        public class LevelFinished
        {
            public int level = 0;
        }
        public LevelFinished level_finished = new LevelFinished();

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
        OnEvent += del;
    }

    public void UnSuscribe(OnEventDel del)
    {
        OnEvent -= del;
    }

    public delegate void OnEventDel(EventManager.Event ev);
    private event OnEventDel OnEvent = null;
}
