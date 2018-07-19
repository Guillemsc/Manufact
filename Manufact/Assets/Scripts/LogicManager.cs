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
    }

    private void Start()
    {
        //StartPhase(LogicPhase.GAME_LOAD_SCREENS);
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

    private void StartGameLoadScreens()
    {
        if (intro_logo_ui != null)
        {
            intro_logo_ui.UIBegin();
        }
    }

    private void StartGameMenu()
    {

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

    }
}
