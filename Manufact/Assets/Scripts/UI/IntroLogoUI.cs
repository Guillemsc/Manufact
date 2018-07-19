using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IntroLogoUI : UIControl
{
    enum IntroLogoState
    {
        WAITING_TO_FADE_IN,
        FADING_IN,
        WAITING_TO_FADE_OUT,
        FADING_OUT,
        BACKGROUND_FADING_OUT,
        FINISHED,
    }

    private IntroLogoState state = IntroLogoState.WAITING_TO_FADE_IN;

    private Timer timer_before_fading_in = new Timer();
    [SerializeField] private float time_before_fading_in = 2.0f;

    private Timer timer_fade_in = new Timer();
    [SerializeField] private float time_fade_in = 1.0f;

    private Timer timer_before_fading_out = new Timer();
    [SerializeField] private float time_before_fading_out = 2.0f;

    private Timer timer_fade_out = new Timer();
    [SerializeField] private float time_fade_out = 1.0f;

    private Timer timer_background_fade_out = new Timer();
    [SerializeField] private float time_background_fade_out = 1.0f;

    [SerializeField] private Image background_image = null;

    [SerializeField] private Image logo_image = null;

    private bool background_fade = false;

    public override void UIBegin()
    {
        base.UIBegin();

        logo_image.color = new Color(logo_image.color.r, logo_image.color.g, logo_image.color.b, 0);
        timer_before_fading_in.Start();
    }

    public override void UIRestart()
    {
        logo_image.color = new Color(logo_image.color.r, logo_image.color.g, logo_image.color.b, 0);
        timer_before_fading_in.Start();
    }

    public void SetBackgroundFade(bool set)
    {
        background_fade = set;
    }

    private void Awake ()
    {
        logo_image.color = new Color(logo_image.color.r, logo_image.color.g, logo_image.color.b, 0);
    }

    private void Start()
    {
        UIBegin();
    }

    private void Update ()
    {
        switch(state)
        {
            case IntroLogoState.WAITING_TO_FADE_IN:
                {
                    if (timer_before_fading_in.ReadTime() > time_before_fading_in)
                    {
                        if (logo_image != null)
                        {
                            logo_image.DOFade(1, time_fade_in);
                        }

                        timer_fade_in.Start();
                        state = IntroLogoState.FADING_IN;
                    }

                    break;
                }
            case IntroLogoState.FADING_IN:
                {
                    if(timer_fade_in.ReadTime() > time_fade_in)
                    {
                        timer_before_fading_out.Start();
                        state = IntroLogoState.WAITING_TO_FADE_OUT;
                    }
                    break;
                }
            case IntroLogoState.WAITING_TO_FADE_OUT:
                {
                    if(timer_before_fading_out.ReadTime() > time_before_fading_out)
                    {
                        if (logo_image != null)
                        {
                            logo_image.DOFade(0, time_fade_out);
                        }

                        timer_fade_out.Start();
                        state = IntroLogoState.FADING_OUT;
                    }
                    break;
                }
            case IntroLogoState.FADING_OUT:
                {
                    if(timer_fade_out.ReadTime() > time_fade_out)
                    {
                        if (background_fade)
                        {
                            state = IntroLogoState.BACKGROUND_FADING_OUT;

                            if (background_image != null)
                                background_image.DOFade(0, time_background_fade_out);

                            timer_background_fade_out.Start();
                        }
                        else
                        {
                            state = IntroLogoState.FINISHED;
                            UIOnFinish();
                        }
                    }
                    break;
                }
            case IntroLogoState.BACKGROUND_FADING_OUT:
                {
                    if (timer_background_fade_out.ReadTime() > time_background_fade_out)
                    {
                        state = IntroLogoState.FINISHED;
                        UIOnFinish();
                    }
                    break;
                }
            case IntroLogoState.FINISHED:
                {
                    break;
                }
        }
    }
}
