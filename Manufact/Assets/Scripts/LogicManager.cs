using System.Collections;
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
    }

    private void Start()
    {
        EventManager.Instance.Suscribe(OnEvent);

        if(AppManager.Instance.GetIsRelease())
        {
            StartPhase(LogicPhase.GAME_LOAD_SCREENS);
        }
        else
            LevelsManager.Instance.StartLevel(15);
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
        LevelsManager.Instance.EndLevel();
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
            main_menu_ui.UIBegin();
        }
    }

    private void OnIntroLogoUIFinished(UIControl c)
    {
        if (select_language_ui != null)
        {
            select_language_ui.UIBegin();
            select_language_ui.SetBackgroundFade(true);
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
                break;
        }
    }

}
