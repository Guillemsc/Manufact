using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LevelStartUI : Singleton<LevelStartUI>
{
    enum LevelStartState
    {
        ALL_BACK_FADE_IN,
        FADING_IN,
        WAITING_TO_FADE_OUT,
        FADING_OUT,
        FINISHED,
    }

    private int level_to_load = 0;

    private LevelStartState state = LevelStartState.FADING_IN;

    [SerializeField] private CanvasGroup canvas_group = null;
    [SerializeField] private TMPro.TextMeshProUGUI number_text = null;
    [SerializeField] private TMPro.TextMeshProUGUI description_text = null;
    [SerializeField] private Image background_image = null;
    [SerializeField] private Image all_back_image = null;

    [SerializeField] private float starting_alpha_val = 0.9f;

    [SerializeField] private float all_back_fade_in_time = 1.0f;
    private Timer all_back_fade_in_timer = new Timer();

    [SerializeField] private float fade_in_time = 1.0f;
    private Timer fade_in_timer = new Timer();

    [SerializeField] private float wait_time = 2.0f;
    private Timer wait_timer = new Timer();

    [SerializeField] private float fade_out_time = 1.0f;
    private Timer fade_out_timer = new Timer();

    private void Awake()
    {
        InitInstance(this, gameObject);

        gameObject.SetActive(false);
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        switch (state)
        {
            case LevelStartState.ALL_BACK_FADE_IN:
                {
                    if(all_back_fade_in_timer.ReadTime() > all_back_fade_in_time)
                    {
                        background_image.transform.DOMoveX(canvas_group.gameObject.transform.position.x, fade_in_time);
                        fade_in_timer.Start();

                        state = LevelStartState.FADING_IN;
                    }

                    break;
                }
            case LevelStartState.FADING_IN:
                {
                    if (fade_in_timer.ReadTime() > fade_in_time)
                    {
                        wait_timer.Start();

                        EventManager.Event ev = new EventManager.Event(EventManager.EventType.LEVEL_LOAD);
                        ev.level_load.level = level_to_load;
                        EventManager.Instance.SendEvent(ev);

                        state = LevelStartState.WAITING_TO_FADE_OUT;
                    }

                    break;
                }
            case LevelStartState.WAITING_TO_FADE_OUT:
                {
                    if (wait_timer.ReadTime() > wait_time)
                    {
                        canvas_group.DOFade(0, fade_out_time);
                        fade_out_timer.Start();

                        state = LevelStartState.FADING_OUT;
                    }

                    break;
                }
            case LevelStartState.FADING_OUT:
                {
                    if (fade_out_timer.ReadTime() > fade_out_time)
                    {
                        state = LevelStartState.FINISHED;
                    }
                    break;
                }

            case LevelStartState.FINISHED:
                {
                    gameObject.SetActive(false);
                    break;
                }
        }
	}

    public void StartLevel(int level_number, string level_text)
    {
        level_to_load = level_number;

        number_text.text = level_number.ToString();
        description_text.text = level_text;

        Vector3 starting_pos = new Vector3(canvas_group.gameObject.transform.position.x + background_image.rectTransform.rect.size.x * 2,
            canvas_group.gameObject.gameObject.transform.position.y, canvas_group.gameObject.transform.position.z);

        background_image.gameObject.transform.position = starting_pos;

        gameObject.SetActive(true);

        canvas_group.alpha = starting_alpha_val;

        all_back_image.color = new Color(all_back_image.color.r, all_back_image.color.g, all_back_image.color.b, 0.0f);
        all_back_image.DOFade(1.0f, all_back_fade_in_time);
        all_back_fade_in_timer.Start();

        state = LevelStartState.ALL_BACK_FADE_IN;
    }
}
