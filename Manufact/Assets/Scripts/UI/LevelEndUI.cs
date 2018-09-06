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
        WATING_TO_FADE_IN,
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
    [SerializeField] private TMPro.TextMeshProUGUI next_level_play_again_text = null;
    [SerializeField] private Button button1 = null;
    [SerializeField] private Button button2 = null;

    [SerializeField] private float starting_alpha_val = 0.9f;

    [SerializeField] private float wait_fade_in_time = 1.0f;
    private Timer wait_fade_in_timer = new Timer();

    [SerializeField] private float all_back_fade_in_time = 1.0f;
    private Timer all_back_fade_in_timer = new Timer();

    [SerializeField] private float fade_in_time = 1.0f;
    private Timer fade_in_timer = new Timer();

    [SerializeField] private float fade_out_time = 1.0f;
    private Timer fade_out_timer = new Timer();

    private bool win = false;
    private bool level_reestart = false;

    void Update()
    {
        switch (state)
        {
            case LevelEndState.WATING_TO_FADE_IN:
                {
                    if (wait_fade_in_timer.ReadTime() > wait_fade_in_time)
                    {
                        all_back_image.gameObject.SetActive(true);

                        if (win)
                        {
                            object[] val = { level_ended };
                            level_status_text.text = LocManager.Instance.GetText("FinishLevelLevelCompleted", val);
                            next_level_play_again_text.text = LocManager.Instance.GetText("FinishLevelNextLevel");
                        }
                        else
                        {
                            object[] val = { level_ended };
                            level_status_text.text = LocManager.Instance.GetText("FinishLevelLevelFailed", val);
                            next_level_play_again_text.text = LocManager.Instance.GetText("FinishLevelReestartLevel");
                        }

                        all_back_image.DOFade(1.0f, all_back_fade_in_time);
                        all_back_fade_in_timer.Start();

                        state = LevelEndState.ALL_BACK_FADE_IN;
                    }

                    break;
                }
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
                        button1.interactable = true;
                        button2.interactable = true;

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
                        if (level_reestart)
                        {
                            EventManager.Event ev = new EventManager.Event(EventManager.EventType.LEVEL_BEGIN);
                            ev.level_begin.level = level_ended;
                            EventManager.Instance.SendEvent(ev);

                            level_reestart = false;
                        }

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
        win = finished;

        gameObject.SetActive(true);

        Canvas.ForceUpdateCanvases();

        Vector3 starting_pos = new Vector3(canvas_group.gameObject.transform.position.x + background_image.rectTransform.rect.size.x * 2,
            canvas_group.gameObject.gameObject.transform.position.y, canvas_group.gameObject.transform.position.z);

        all_back_image.transform.position = canvas_group.gameObject.transform.position;
        background_image.gameObject.transform.position = starting_pos;
        all_back_image.color = new Color(all_back_image.color.r, all_back_image.color.g, all_back_image.color.b, 0.0f);

        canvas_group.alpha = starting_alpha_val;

        button1.interactable = false;
        button2.interactable = false;

        wait_fade_in_timer.Start();

        state = LevelEndState.WATING_TO_FADE_IN;
    }

    public void FadeOut(bool _level_reestart)
    {
        level_reestart = _level_reestart;

        gameObject.SetActive(true);

        Vector3 finish_pos = new Vector3(canvas_group.gameObject.transform.position.x - background_image.rectTransform.rect.size.x * 2,
        canvas_group.gameObject.gameObject.transform.position.y, canvas_group.gameObject.transform.position.z);

        background_image.transform.DOMoveX(finish_pos.x, fade_out_time);
        all_back_image.transform.DOMoveX(finish_pos.x, fade_out_time);

        button1.interactable = false;
        button2.interactable = false;

        fade_out_timer.Start();

        state = LevelEndState.FADING_OUT;
    }

    public void NextLevelReplayButtonDow()
    {
        LogicManager.Instance.NextLevelReestart(win);
    }
}
