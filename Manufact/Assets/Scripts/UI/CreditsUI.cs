using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CreditsUI : MonoBehaviour
{
    enum CreditsState
    {
        FADING_IN,
        WAITING_TO_FADE_OUT,
        FADING_OUT,
        FINISHED,
    }

    CreditsState state = new CreditsState();

    [SerializeField] private TMPro.TextMeshProUGUI version_text;

    [SerializeField] private CanvasGroup canvas_group = null;
    [SerializeField] private Image background_image = null;

    [SerializeField] private float fade_in_time = 1.0f;
    private Timer fade_in_timer = new Timer();

    [SerializeField] private float fade_out_time = 1.0f;
    private Timer fade_out_timer = new Timer();

    private void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        switch(state)
        {
            case CreditsState.FADING_IN:
                if(fade_in_timer.ReadTime() > fade_in_time)
                {
                    state = CreditsState.WAITING_TO_FADE_OUT;
                }
                break;
            case CreditsState.WAITING_TO_FADE_OUT:
                break;
            case CreditsState.FADING_OUT:
                if (fade_out_timer.ReadTime() > fade_out_time)
                {
                    gameObject.SetActive(false);
                    state = CreditsState.FINISHED;
                }
                break;
            case CreditsState.FINISHED:
                break;
        }
    }

    public void FadeIn()
    {
        if (state != CreditsState.FADING_OUT)
        {
            gameObject.SetActive(true);

            version_text.text = AppManager.Instance.GetVersion();

            Canvas.ForceUpdateCanvases();

            Vector3 starting_pos = new Vector3(canvas_group.gameObject.transform.position.x + (background_image.rectTransform.rect.width * 0.6f),
                canvas_group.gameObject.gameObject.transform.position.y, canvas_group.gameObject.transform.position.z);

            background_image.gameObject.transform.position = starting_pos;

            background_image.transform.DOMoveX(canvas_group.gameObject.transform.position.x, fade_in_time);

            fade_in_timer.Start();

            state = CreditsState.FADING_IN;
        }
    }

    public void FadeOut()
    {
        if (state == CreditsState.WAITING_TO_FADE_OUT)
        {
            Vector3 finish_pos = new Vector3(canvas_group.gameObject.transform.position.x - (background_image.rectTransform.rect.width * 0.6f),
            canvas_group.gameObject.gameObject.transform.position.y, canvas_group.gameObject.transform.position.z);

            background_image.transform.DOMoveX(finish_pos.x, fade_out_time);

            fade_out_timer.Start();

            state = CreditsState.FADING_OUT;
        }
    }
}
