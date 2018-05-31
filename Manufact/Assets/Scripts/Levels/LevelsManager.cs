using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : Singleton<LevelsManager>
{
    private List<Level> levels = new List<Level>();

    private Level current_level = null;

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
    }

    private void Update()
    {
        CheckCurrentLevelStates();
    }

    private void CheckCurrentLevelStates()
    {
        if (current_level != null)
        {
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
                current_level.OnEnd();
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
        current_level = null;

        for (int i = 0; i < levels.Count; ++i)
        {
            Level curr_level = levels[i];

            if (curr_level.GetLevelNumber() == level_number)
            {
                current_level = curr_level;
                break;
            }
        }

        if (current_level != null)
        {
            current_level.OnStart();
        }
        else
        {
            Debug.LogError("[Level] Level could be started, number not found: " + level_number);
        }
    }
}
