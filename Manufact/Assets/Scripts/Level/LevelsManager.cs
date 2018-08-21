using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : Singleton<LevelsManager>
{
    private List<Level> levels = new List<Level>();

    private Level current_level = null;

    int  level_to_start = 0;
    bool to_start_level = false;

    [SerializeField] LevelStartUI level_start_ui = null;

    private void Awake()
    {
        InitInstance(this, gameObject);

        Level[] levels_arr = FindObjectsOfType<Level>();

        for (int i = 0; i < levels_arr.Length; ++i)
        {
            Level curr_level = levels_arr[i];

            if (!CheckNumberDuplicate(curr_level.GetLevelNumber()))
            {
                levels.Add(curr_level);
            }
            else
            {
                Debug.LogError("[Levels] There are two levels with the same number: " + curr_level.GetLevelNumber());
            }
        }

        level_start_ui.gameObject.SetActive(false);
    }

    private void Start()
    {
        EventManager.Instance.Suscribe(OnEvent);
    }

    private void Update()
    {
        if(to_start_level)
        {
            ActuallyStartLevel();
            to_start_level = false;
        }

        CheckCurrentLevelStates();
    }

    private void CheckCurrentLevelStates()
    {
        if (current_level != null)
        {
            current_level.OnUpdate();

            bool finished = false;

            if (current_level.OnCheckWin())
            {
                // Win
                finished = true;
            }
            else if (current_level.OnCheckLose())
            {
                // Lose
                finished = true;
            }

            if (finished)
            {
                EventManager.Event ev = new EventManager.Event(EventManager.EventType.LEVEL_FINISHED);
                ev.level_finished.level = current_level.GetLevelNumber();
                EventManager.Instance.SendEvent(ev);

                current_level.OnEnd();
                current_level.gameObject.SetActive(false);
                current_level = null;
            }
        }
    }

    private bool CheckNumberDuplicate(int number)
    {
        bool ret = false;

        for (int i = 0; i < levels.Count; ++i)
        {
            if (levels[i].GetLevelNumber() == number)
            {
                ret = true;
                break;
            }
        }

        return ret;
    }

    public Level GetCurrentLevel()
    {
        return current_level;
    }

    public void StartLevel(int level_number)
    {
        level_to_start = level_number;
        to_start_level = true;
    }

    public void ActuallyStartLevel()
    {
        current_level = null;

        for (int i = 0; i < levels.Count; ++i)
        {
            Level level = levels[i];

            if (level.GetLevelNumber() == level_to_start)
            {
                current_level = level;
                break;
            }
        }

        if (current_level != null)
        {
            level_start_ui.StartLevel(current_level.GetLevelNumber(), current_level.GetLevelName(), current_level.GetLevelDescription());

            current_level.OnAwake();
        }
        else
        {
            Debug.LogError("[Level] Level could not be started, number not found: " + level_to_start);
        }
    }

    private void OnEvent(EventManager.Event ev)
    {
        switch(ev.Type())
        {
            case EventManager.EventType.LEVEL_LOAD:

                if (current_level != null)
                {
                    current_level.gameObject.SetActive(true);

                    current_level.OnStart();

                    EventManager.Event new_ev = new EventManager.Event(EventManager.EventType.LEVEL_STARTED);
                    new_ev.level_started.level = current_level.GetLevelNumber();
                    EventManager.Instance.SendEvent(new_ev);
                }
                break;
        }
    }
}
