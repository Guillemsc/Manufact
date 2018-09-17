using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : Singleton<LevelsManager>
{
    private List<LevelsStage> level_stages = new List<LevelsStage>();

    private LevelsStage curr_level_stage = null;
    private Level current_level = null;
    private Level last_level = null;

    private int  level_to_start = 0;
    private int  level_stage_to_start = 0;
    private bool to_start_level = false;
    private bool level_to_start_use_intro = false;

    [SerializeField] private LevelStartUI level_start_ui = null;
    [SerializeField] private LevelEndUI level_end_ui = null;
    [SerializeField] private LevelsUI levels_ui = null;

    public class LevelsStage
    {
        public int stage = 0;

        public Level GetLevel(int level_index)
        {
            Level ret = null;

            for (int i = 0; i < levels.Count; ++i)
            {
                Level level = levels[i];

                if (level.GetLevelNumber() == level_index)
                {
                    ret = level;
                    break;
                }
            }

            return ret;
        }

        public void AddLevel(Level level)
        {
            if (level != null)
            {
                if (GetLevel(level.GetLevelNumber()) == null)
                {
                    if (level.GetLevelStage() == stage)
                        levels.Add(level);
                }
                else
                {
                    Debug.LogError("[Levels] There are two levels with the same number: " + level.GetLevelNumber());
                }
            }
        }

        public List<Level> levels = new List<Level>();
    }

    private void Awake()
    {
        InitInstance(this, gameObject);

        Level[] levels_arr = (Level[])Resources.FindObjectsOfTypeAll(typeof(Level));

        for (int i = 0; i < levels_arr.Length; ++i)
        {
            Level curr_level = levels_arr[i];

            int stage_index = curr_level.GetLevelStage();

            LevelsStage stage = GetLevelStage(stage_index);

            if(stage != null)
            {
                stage.AddLevel(curr_level);
            }
            else
            {
                LevelsStage new_stage = new LevelsStage();
                new_stage.stage = stage_index;

                level_stages.Add(new_stage);

                new_stage.AddLevel(curr_level);
            }
        }

        level_start_ui.gameObject.SetActive(false);
        level_end_ui.gameObject.SetActive(false);
        levels_ui.gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < level_stages.Count; ++i)
        {
            LevelsStage stage = level_stages[i];

            for(int y = 0; y < stage.levels.Count; ++y)
            {
                stage.levels[y].gameObject.SetActive(false);
            }
        }

        EventManager.Instance.Suscribe(OnEvent);

        StartStage(0);
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

    public LevelEndUI GetLevelEndUI()
    {
        return level_end_ui;
    }

    private void CheckCurrentLevelStates()
    {
        if (current_level != null && current_level.GetStarted())
        {
            current_level.OnUpdate();

            bool finished = false;
            bool win = false;

            if (current_level.OnCheckWin())
            {
                // Win
                finished = true;
                win = true;
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
                ev.level_finished.win = win;
                EventManager.Instance.SendEvent(ev);

                level_end_ui.EndLevel(win, current_level.GetLevelNumber());

                EndLevel();
            }
        }
    }

    public LevelsStage GetCurrentStage()
    {
        return curr_level_stage;
    }

    public Level GetCurrentLevel()
    {
        return current_level;
    }

    public LevelsStage GetLevelStage(int stage)
    {
        LevelsStage ret = null;

        for(int i = 0; i < level_stages.Count; ++i)
        {
            LevelsStage curr_stage = level_stages[i];

            if (curr_stage.stage == stage)
            {
                ret = curr_stage;
                break;
            }
        }

        return ret;
    }

    public Level GetCurrentStageLevel(int level)
    {
        Level ret = null;

        if (curr_level_stage != null)
        {
            ret = curr_level_stage.GetLevel(level);
        }

        return ret;
    }

    public int GetStageLevelsCount(int stage)
    {
        int ret = 0;

        LevelsStage curr_stage = GetLevelStage(stage);

        if(curr_stage != null)
        {
            ret = curr_stage.levels.Count;
        }

        return ret;
    }

    public bool StartStage(int stage)
    {
        bool ret = false;

        LevelsStage curr_stage = GetLevelStage(stage);

        if(curr_stage != null)
        {
            curr_level_stage = curr_stage;

            ret = true;
        }

        return ret;
    }

    public bool StartNextStage()
    {
        bool ret = false;

        if(curr_level_stage != null)
        {
            ret = StartStage(curr_level_stage.stage + 1);
        }

        return ret;
    }

    public bool StartLevel(int level_number, bool use_intro = true)
    {
        bool ret = false;

        if (curr_level_stage != null)
        {
            Level level = GetLevel(curr_level_stage.stage, level_number);

            if (level != null)
            {
                level.gameObject.SetActive(false);
                level_to_start = level_number;
                level_stage_to_start = curr_level_stage.stage;
                to_start_level = true;
                level_to_start_use_intro = use_intro;

                ret = true;
            }
        }

        return ret;
    }

    public bool StartNextLevel()
    {
        bool ret = false;

        if (last_level != null)
        {
            ret = StartLevel(last_level.GetLevelNumber() + 1);
        }

        return ret;
    }

    public bool ReestartLastLevel()
    {
        bool ret = false;

        if (last_level != null)
            ret = StartLevel(last_level.GetLevelNumber(), false);

        return ret;
    }

    public bool ReestartCurrentLevel()
    {
        bool ret = false;

        if(current_level != null)
            ret = StartLevel(current_level.GetLevelNumber(), false);

        return ret;
    }

    public void LoadLevel()
    {
        if(current_level != null)
        {
            current_level.gameObject.SetActive(true);
        }
    }

    public void BeginLevel()
    {
        if (current_level != null)
        {
            levels_ui.UIBegin(current_level.GetLevelNumber());
        }
    }

    public void EndLevel()
    {
        if(current_level != null)
        {
            current_level.SetStarted(false);
            current_level.OnEnd();

            levels_ui.FadeOut();
        }
    }

    public void UnloadLevel()
    {
        if (current_level != null)
        {
            current_level.gameObject.SetActive(false);

            last_level = current_level;
            current_level = null;
        }
    }

    public Level GetLevel(int level_stage, int level_num)
    {
        Level ret = null;

        LevelsStage curr_stage = GetLevelStage(level_stage);

        if (curr_stage != null)
        {
            for (int i = 0; i < curr_stage.levels.Count; ++i)
            {
                Level level = curr_stage.levels[i];

                if (level.GetLevelNumber() == level_num)
                {
                    ret = level;
                    break;
                }
            }
        }

        return ret;
    }

    private void ActuallyStartLevel()
    {
        current_level = null;

        current_level = GetLevel(level_stage_to_start, level_to_start);

        if (current_level != null)
        {
            if (level_to_start_use_intro)
            {
                level_start_ui.UIBegin(current_level.GetLevelNumber());

                current_level.OnAwake();

                current_level.OnStart();

                current_level.SetStarted(true);

                EventManager.Event new_ev = new EventManager.Event(EventManager.EventType.LEVEL_STARTED);
                new_ev.level_started.level = current_level.GetLevelNumber();
                EventManager.Instance.SendEvent(new_ev);
            }
            else
            {
                current_level.OnAwake();

                current_level.OnStart();

                current_level.SetStarted(true);

                EventManager.Event new_ev = new EventManager.Event(EventManager.EventType.LEVEL_STARTED);
                new_ev.level_started.level = current_level.GetLevelNumber();
                EventManager.Instance.SendEvent(new_ev);

                EventManager.Event ev = new EventManager.Event(EventManager.EventType.LEVEL_LOAD);
                ev.level_load.level = current_level.GetLevelNumber();
                EventManager.Instance.SendEvent(ev);
            }
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

                LoadLevel();
                break;
            case EventManager.EventType.LEVEL_BEGIN:

                BeginLevel();
                break;

            case EventManager.EventType.LEVEL_UNLOAD:

                UnloadLevel();
                break;
        }
    }
}
