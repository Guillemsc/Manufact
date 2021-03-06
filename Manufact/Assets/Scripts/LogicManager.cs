﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : Singleton<LogicManager>
{
    public enum LogicPhase
    {
        GAME_LOAD_SCREENS,
        GAME_MENU,
    }

    private LogicPhase phase = new LogicPhase();

    [SerializeField] private IntroLogoUI intro_logo_ui = null;
    [SerializeField] private LanguageSelectUI select_language_ui = null;
    [SerializeField] private MainMenuUI main_menu_ui = null;
    [SerializeField] private OptionsMenuUI options_menu_ui = null;
    [SerializeField] private StageSelectionUI stage_selection_ui = null;
    [SerializeField] private CreditsUI credits_ui = null;

    private void Awake()
    {
        InitInstance(this, gameObject);

        if (intro_logo_ui != null)
        {
            intro_logo_ui.gameObject.SetActive(false);
            intro_logo_ui.UISuscribeOnFinish(OnIntroLogoUIFinished);
            intro_logo_ui.SetDisableOnFinish(true);
        }

        if (select_language_ui != null)
        {
            select_language_ui.gameObject.SetActive(false);
            select_language_ui.UISuscribeOnFinish(OnSelectLanguageUIFinished);
        }

        if(main_menu_ui != null)
        {
            main_menu_ui.gameObject.SetActive(false);
        }

        if(options_menu_ui != null)
        {
            options_menu_ui.gameObject.SetActive(false);
        }

        if(stage_selection_ui != null)
        {
            stage_selection_ui.gameObject.SetActive(false);
        }

        if(credits_ui != null)
        {
            credits_ui.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        EventManager.Instance.Suscribe(OnEvent);

        if(AppManager.Instance.GetIsRelease())
        {
            StartPhase(LogicPhase.GAME_LOAD_SCREENS);
        }
        else
            StartPhase(LogicPhase.GAME_MENU);
        //LevelsManager.Instance.StartLevel(0);
    }

    public void StartPhase(LogicPhase p)
    {
        switch(p)
        {
            case LogicPhase.GAME_LOAD_SCREENS:
                {
                    StartGameLoadScreens();
                    break;
                }

            case LogicPhase.GAME_MENU:
                {
                    StartGameMenu();
                    break;
                }
        }
    }

    public void MainMenuButtonClick()
    {
        LevelsManager.Instance.StartLevel(0);
    }

    public void NextLevelReestart(bool win)
    {
        bool reestart = false;

        if (win)
        {
            if (!LevelsManager.Instance.StartNextLevel())
            {
                StartGameMenu();
            }
        }
        else
        {
            if (LevelsManager.Instance.ReestartLastLevel())
                reestart = true;
        }

        LevelsManager.Instance.GetLevelEndUI().FadeOut(reestart);
    }

    public void ReturnMainMenuLevelEnd()
    {
        LevelsManager.Instance.GetLevelEndUI().FadeOut(false);
        StartGameMenu();
    }

    public void ReturnMainMenu()
    {
        StartGameMenu();
        LevelsManager.Instance.EndCurrentLevel();
    }

    public void ReestartLevel()
    {
        LevelsManager.Instance.ReestartCurrentLevel();
    }

    public void OpenOptionsMenu()
    {
        options_menu_ui.FadeIn();
    }

    public void CloseOptionsMenu()
    {
        options_menu_ui.FadeOut();
    }

    public void OpenStageSelection()
    {
        stage_selection_ui.FadeIn();
    }

    public void CloseStageSelection()
    {
        stage_selection_ui.FadeOut();
    }

    public void OpenCredits()
    {
        credits_ui.FadeIn();
    }

    public void CloseCredits()
    {
        credits_ui.FadeOut();
    }

    public void ChangeLanguageOptions()
    {
        options_menu_ui.gameObject.SetActive(false);
        select_language_ui.UIBegin();
        select_language_ui.SetBackgroundFade(true);
    }


    private void StartGameLoadScreens()
    {
        if (intro_logo_ui != null)
        {
            intro_logo_ui.UIBegin();
        }
    }

    private void StartGameMenu()
    {
        if (main_menu_ui != null)
        {
            bool first = SerializationManager.Instance.GetFistTime();

            if (first)
                SerializationManager.Instance.SetFirstTime();

            main_menu_ui.FadeIn(!first);
        }
    }

    private void OnIntroLogoUIFinished(UIControl c)
    {
        bool has_language = SerializationManager.Instance.GetHasLanguage();

        if (!has_language)
        {
            if (select_language_ui != null)
            {
                select_language_ui.UIBegin();
                select_language_ui.SetBackgroundFade(true);
            }
        }
        else
        {
            LocManager.Language lan = SerializationManager.Instance.GetLanguage();
            LocManager.Instance.SetLanguage(lan);

            StartPhase(LogicPhase.GAME_MENU);
        }
    }

    private void OnSelectLanguageUIFinished(UIControl c)
    {
        GooglePlayManager.Instance.LogIn();

        StartPhase(LogicPhase.GAME_MENU);
    }

    private void OnEvent(EventManager.Event ev)
    {
        switch(ev.Type())
        {
            case EventManager.EventType.LEVEL_LOAD:
                main_menu_ui.gameObject.SetActive(false);
                stage_selection_ui.gameObject.SetActive(false);
                break;
        }
    }

}
