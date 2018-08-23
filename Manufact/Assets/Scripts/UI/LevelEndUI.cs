using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LevelEndUI : MonoBehaviour
{

    enum LevelEndState
    {
        ALL_BACK_FADE_IN,
        FADING_IN,
        WAITING_TO_FADE_OUT,
        FADING_OUT,
        FINISHED,
    }

    private int level_ended = 0;

    private LevelEndState state = LevelEndState.FADING_IN;

    [SerializeField] private CanvasGroup canvas_group = null;
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private TMPro.TextMeshProUGUI level_status_text = null;
    [SerializeField] private Image background_image = null;
    [SerializeField] private Image all_back_image = null;

    [SerializeField] private float starting_alpha_val = 0.9f;

    [SerializeField] private float all_back_fade_in_time = 1.0f;
    private Timer all_back_fade_in_timer = new Timer();

    [SerializeField] private float fade_in_time = 1.0f;
    private Timer fade_in_timer = new Timer();

    [SerializeField] private float fade_out_time = 1.0f;
    private Timer fade_out_timer = new Timer();

    void Update()
    {
        switch (state)
        {
            case LevelEndState.ALL_BACK_FADE_IN:
                {
                    if (all_back_fade_in_timer.ReadTime() > all_back_fade_in_time)
                    {
                        background_image.transform.DOMoveX(canvas_group.gameObject.transform.position.x, fade_in_time);
                        fade_in_timer.Start();

                        state = LevelEndState.FADING_IN;
                    }

                    break;
                }
            case LevelEndState.FADING_IN:
                {
                    if (fade_in_timer.ReadTime() > fade_in_time)
                    {
                        EventManager.Event ev = new EventManager.Event(EventManager.EventType.LEVEL_UNLOAD);
                        ev.level_unload.level = level_ended;
                        EventManager.Instance.SendEvent(ev);

                        state = LevelEndState.WAITING_TO_FADE_OUT;
                    }

                    break;
                }
            case LevelEndState.WAITING_TO_FADE_OUT:
                {

                    break;
                }
            case LevelEndState.FADING_OUT:
                {
                    if (fade_out_timer.ReadTime() > fade_out_time)
                    {
                        state = LevelEndState.FINISHED;
                    }
                    break;
                }

            case LevelEndState.FINISHED:
                {
                    gameObject.SetActive(false);
                    break;
                }
        }
    }

    public void EndLevel(bool finished, int level_to_end)
    {
        gameObject.SetActive(true);
        all_back_image.gameObject.SetActive(true);

        level_ended = level_to_end;

        if(finished)
        {
            level_status_text.text = LocManager.Instance.GetText("FinishLevelLevelCompleted");
        }
        else
        {
            level_status_text.text = LocManager.Instance.GetText("FinishLevelLevelFailed");
        }

        Canvas.ForceUpdateCanvases();

        Vector3 starting_pos = new Vector3(canvas_group.gameObject.transform.position.x + background_image.rectTransform.rect.size.x * 2,
            canvas_group.gameObject.gameObject.transform.position.y, canvas_group.gameObject.transform.position.z);

        all_back_image.transform.position = canvas_group.gameObject.transform.position;
        background_image.gameObject.transform.position = starting_pos;

        canvas_group.alpha = starting_alpha_val;

        all_back_image.color = new Color(all_back_image.color.r, all_back_image.color.g, all_back_image.color.b, 0.0f);
        all_back_image.DOFade(1.0f, all_back_fade_in_time);
        all_back_fade_in_timer.Start();

        state = LevelEndState.ALL_BACK_FADE_IN;
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);

        Vector3 finish_pos = new Vector3(canvas_group.gameObject.transform.position.x - background_image.rectTransform.rect.size.x * 2,
        canvas_group.gameObject.gameObject.transform.position.y, canvas_group.gameObject.transform.position.z);

        background_image.transform.DOMoveX(finish_pos.x, fade_out_time);
        all_back_image.transform.DOMoveX(finish_pos.x, fade_out_time);

        fade_out_timer.Start();

        state = LevelEndState.FADING_OUT;
    }
}
