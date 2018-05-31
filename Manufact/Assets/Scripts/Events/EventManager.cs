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
    }

    public class Event
    {
        public Event(EventManager.EventType e_type)
        {
            event_type = e_type;
        }

        // Map
        public class MapEntitySpawn
        {
            public MapEntity entity = null;
        } public MapEntitySpawn map_entity_spawn = new MapEntitySpawn();


        public class MapEntityDelete
        {
            public MapEntity entity = null;
        }
        public MapEntityDelete map_entity_delete = new MapEntityDelete();

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
