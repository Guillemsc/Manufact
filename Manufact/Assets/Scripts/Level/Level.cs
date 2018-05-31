using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Base level info")]
    [SerializeField] private string level_name = "no_name_key";
    [SerializeField] private string level_description = "no_description_key";
    [SerializeField] private int level_number = 0;

    public string GetLevelName()
    {
        return LocManager.Instance.GetText(level_name);
    }

    public string GetLevelDescription()
    {
        return LocManager.Instance.GetText(level_description);
    }

    public int GetLevelNumber()
    {
        return level_number;
    }

    public virtual void OnAwake()
    {

    }

    public virtual void OnStart()
    {

    }

    public virtual void OnEnd()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual bool OnCheckWin()
    {
        return false;
    }

    public virtual bool OnCheckLose()
    {
        return false;
    }
}
