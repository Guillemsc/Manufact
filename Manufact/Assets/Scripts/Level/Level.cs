﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Base level info")]
    [SerializeField] private string level_name = "no_name_key";
    [SerializeField] private string level_description = "no_description_key";

    [SerializeField] private int level_stage = 0;
    [SerializeField] private int level_number = 0;

    private bool completed = false;

    [Header("Build info")]
    [SerializeField] protected GameGridInstance grid = null;
    [SerializeField] protected EntityPathInstance path = null;

    protected bool started = false;

    public string GetLevelName()
    {
        return LocManager.Instance.GetText(level_name);
    }

    public string GetLevelDescription()
    {
        return LocManager.Instance.GetText(level_description);
    }

    public int GetLevelStage()
    {
        return level_stage;
    }

    public int GetLevelNumber()
    {
        return level_number;
    }

    public void SetCompleted(bool set)
    {
        completed = set;
    }

    public bool GetCompleted()
    {
        return completed;
    }

    public void SetStarted(bool set)
    {
        started = set;
    }

    public bool GetStarted()
    {
        return started;
    }

    public void Awake()
    {
      
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
