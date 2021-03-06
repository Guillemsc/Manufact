﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private GameObject debug_ui_go = null;
    [SerializeField] private TMPro.TextMeshProUGUI fps_text = null;
    [SerializeField] private TMPro.TextMeshProUGUI version_text = null;
    [SerializeField] private TMPro.TextMeshProUGUI curr_level_text = null;

    private void Start()
    {
        if (debug_ui_go != null)
            debug_ui_go.SetActive(!AppManager.Instance.GetIsRelease());

        if (version_text != null)
            version_text.text = "V." + AppManager.Instance.GetVersion().ToString();
    }

    private void Update()
    {
        if (fps_text != null)
            fps_text.text = "FPS: " + AppManager.Instance.GetFPS().ToString();

        Level lev = LevelsManager.Instance.GetCurrentLevel();
        if (lev != null)
        {
            if(curr_level_text != null)
            {
                curr_level_text.text = "Level: " + lev.GetLevelNumber();
            }
            else
            {
                curr_level_text.text = "Level: null";
            }
        }
    }
}
