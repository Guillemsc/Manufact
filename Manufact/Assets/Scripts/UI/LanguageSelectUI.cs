using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LanguageSelectUI : UIControl
{
    enum LanguageSelectState
    {
        WAITING_TO_FADE_IN,
        FADING_IN,
        WAITING_TO_FADE_OUT,
        FADING_OUT,
        BACKGROUND_FADING_OUT,
        FINISHED,
    }

    private LanguageSelectState state = LanguageSelectState.WAITING_TO_FADE_IN;

    private Timer timer_before_fading_in = new Timer();
    [SerializeField] private float time_before_fading_in = 2.0f;

    private Timer timer_fade_in = new Timer();
    [SerializeField] private float time_fade_in = 1.0f;

    private Timer timer_fade_out = new Timer();
    [SerializeField] private float time_fade_out = 1.0f;

    private Timer timer_background_fade_out = new Timer();
    [SerializeField] private float time_background_fade_out = 1.0f;

    [SerializeField] private Button english_button = null;

    [SerializeField] private Button spanish_button = null;

    [SerializeField] private Image background_image = null;
    [SerializeField] private CanvasGroup elements_group = null;

    private bool background_fade = false;

    public override void UIBegin()
    {
        base.UIBegin();

        elements_group.alpha = 0.0f;

        timer_before_fading_in.Start();
    }

    public override void UIRestart()
    {

    }

    public void SetBackgroundFade(bool set)
    {
        background_fade = set;
    }

    private void Awake()
    {
        if (english_button != null)
            english_button.onClick.AddListener(OnEnglishPress);

        if (spanish_button != null)
            spanish_button.onClick.AddListener(OnSpanishPress);

        if(elements_group != null)
            elements_group.alpha = 0.0f;
    }

    // Use this for initialization
    void Start ()
    {
        UIBegin();
	}

    // Update is called once per frame
    void Update()
    {       
       switch (state)
       {
           case LanguageSelectState.WAITING_TO_FADE_IN:
               {
                   if (timer_before_fading_in.ReadTime() > time_before_fading_in)
                   {
                        if (elements_group != null)
                            elements_group.DOFade(1, time_fade_in);

                        timer_fade_in.Start();
                        state = LanguageSelectState.FADING_IN;
                   }

                   break;
               }
           case LanguageSelectState.FADING_IN:
               {
                   if (timer_fade_in.ReadTime() > time_fade_in)
                   {
                       state = LanguageSelectState.WAITING_TO_FADE_OUT;
                   }
                   break;
               }
           case LanguageSelectState.FADING_OUT:
               {
                   if (timer_fade_out.ReadTime() > time_fade_out)
                   {
                        if (background_fade)
                        {
                            state = LanguageSelectState.BACKGROUND_FADING_OUT;

                            if (background_image != null)
                                background_image.DOFade(0, time_background_fade_out);

                            timer_background_fade_out.Start();
                        }
                        else
                        {
                            state = LanguageSelectState.FINISHED;
                            UIOnFinish();
                        }
                    }
                   break;
               }
            case LanguageSelectState.BACKGROUND_FADING_OUT:
                {
                    if(timer_background_fade_out.ReadTime() > time_background_fade_out)
                    {
                        state = LanguageSelectState.FINISHED;
                        UIOnFinish();
                    }
                    break;
                }
            case LanguageSelectState.FINISHED:
               {
                   break;
               }
       }
    }

    private void FinishWaitingToFadeOut()
    {
        if (elements_group != null)
            elements_group.DOFade(0, time_fade_out);

        state = LanguageSelectState.FADING_OUT;
        timer_fade_out.Start();
    }

    private void OnEnglishPress()
    {        
        LocManager.Instance.SetLanguage(LocManager.Language.EN);

        FinishWaitingToFadeOut();
    }

    private void OnSpanishPress()
    {        
        LocManager.Instance.SetLanguage(LocManager.Language.SPA);

        FinishWaitingToFadeOut();
    }
}
