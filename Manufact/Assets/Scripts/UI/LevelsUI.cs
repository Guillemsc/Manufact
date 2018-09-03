using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelsUI : UIControl
{
    enum LevelUIState
    {
        FADING_IN,
        FADING_OUT,
        FINISHED,
    }

    private LevelUIState state = new LevelUIState();

    [SerializeField] private CanvasGroup canvas_group = null;

    private Timer fade_in_timer = new Timer();
    [SerializeField] private float fade_in_time = 0.4f;

    private Timer fade_out_timer = new Timer();
    [SerializeField] private float fade_out_time = 0.4f;

    void Update()
    {
        switch (state)
        {
            case LevelUIState.FADING_IN:
                {
                    if (fade_in_timer.ReadTime() > fade_in_time)
                    {

                    }

                    break;
                }
            case LevelUIState.FADING_OUT:
                {
                    if (fade_out_timer.ReadTime() > fade_out_time)
                    {
                        gameObject.SetActive(false);
                        state = LevelUIState.FINISHED;
                    }

                    break;
                }
            case LevelUIState.FINISHED:
                break;

        }
    }

    public override void UIBegin()
    {
        base.UIBegin();

        gameObject.SetActive(true);

        canvas_group.alpha = 0.0f;
        canvas_group.DOFade(1, fade_in_time);
        fade_in_timer.Start();

        state = LevelUIState.FADING_IN;
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);

        canvas_group.DOFade(0, fade_out_time);
        fade_out_timer.Start();

        state = LevelUIState.FADING_OUT;
    }
}
