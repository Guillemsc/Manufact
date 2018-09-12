using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionsMenuUI : MonoBehaviour
{
    enum OptionsMenuSelectState
    {
        WATING_TO_FADE_IN,
        FADING_IN,
        WAITING_TO_FADE_OUT,
        FADING_OUT,
    }

    private OptionsMenuSelectState state = OptionsMenuSelectState.WATING_TO_FADE_IN;

    private Timer fade_in_timer = new Timer();
    [SerializeField ] private float fade_in_time = 0.0f;

    private Timer fade_out_timer = new Timer();
    [SerializeField] private float fade_out_time = 0.0f;

    [SerializeField] private CanvasGroup canvas_group = null;
    [SerializeField] private Image background_image = null;

    private void Start ()
    {
        Vector3 starting_pos = new Vector3(canvas_group.gameObject.transform.position.x,
        canvas_group.gameObject.gameObject.transform.position.y + background_image.rectTransform.rect.size.y * 2,
        canvas_group.gameObject.transform.position.z);

        background_image.gameObject.transform.position = starting_pos;
    }
	
	private void Update ()
    {
		switch(state)
        {
            case OptionsMenuSelectState.WATING_TO_FADE_IN:
                break;
            case OptionsMenuSelectState.FADING_IN:
                if(fade_in_timer.ReadTime() > fade_in_time)
                {
                    state = OptionsMenuSelectState.WAITING_TO_FADE_OUT;
                }
                break;
            case OptionsMenuSelectState.WAITING_TO_FADE_OUT:
                break;
            case OptionsMenuSelectState.FADING_OUT:
                if (fade_out_timer.ReadTime() > fade_out_time)
                {
                    state = OptionsMenuSelectState.WATING_TO_FADE_IN;
                    gameObject.SetActive(false);
                }

                break;
        }
	}

    public void FadeIn()
    {
        if (state != OptionsMenuSelectState.FADING_OUT)
        { 
            gameObject.SetActive(true);

            Canvas.ForceUpdateCanvases();

            fade_in_timer.Start();
            state = OptionsMenuSelectState.FADING_IN;

            Vector3 starting_pos = new Vector3(canvas_group.gameObject.transform.position.x,
            canvas_group.gameObject.gameObject.transform.position.y + background_image.rectTransform.rect.size.y,
            canvas_group.gameObject.transform.position.z);

            background_image.gameObject.transform.position = starting_pos;

            background_image.transform.DOMoveY(canvas_group.gameObject.transform.position.y, fade_in_time);
            fade_in_timer.Start();
        }
    }

    public void FadeOut()
    {
        if (state != OptionsMenuSelectState.FADING_IN)
        {
            fade_out_timer.Start();
            state = OptionsMenuSelectState.FADING_OUT;

            Vector3 finish_pos = new Vector3(canvas_group.gameObject.transform.position.x,
            canvas_group.gameObject.gameObject.transform.position.y + background_image.rectTransform.rect.size.y,
            canvas_group.gameObject.transform.position.z);

            background_image.transform.DOMoveY(finish_pos.y, fade_out_time);
        }
    }
}
