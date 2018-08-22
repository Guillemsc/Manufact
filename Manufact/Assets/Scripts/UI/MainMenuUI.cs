using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuUI : UIControl
{
    enum MainMenuSelectState
    {
        FADING_IN,
        TRIANGLES_FADING_IN,
        FADING_OUT,
        FINISHED,
    }

    private MainMenuSelectState state = MainMenuSelectState.FADING_IN;

    [SerializeField] private Image logo_image = null;

    [SerializeField] private GameObject logo_group = null;

    [SerializeField] private GameObject logo_starting_pos = null;
    [SerializeField] private GameObject logo_move_in_pos = null;
    [SerializeField] private GameObject logo_move_out_pos = null;

    [SerializeField] private float logo_move_in_time = 1.0f;
    [SerializeField] private float logo_move_out_time = 1.0f;

    [SerializeField] private GameObject blue_triangle = null;
    [SerializeField] private float blue_time_offset_move_in_time = 1.0f;
    private Timer blue_time_offset_in_timer = new Timer();
    [SerializeField] private float blue_triangle_move_in_time = 1.0f;
    private Timer blue_triangle_move_in_timer = new Timer();

    [SerializeField] private GameObject orange_triangle = null;
    [SerializeField] private float orange_time_offset_move_in_time = 1.5f;
    private Timer orange_time_offset_move_in_timer = new Timer();
    [SerializeField] private float orange_triangle_move_in_time = 1.5f;
    private Timer orange_triangle_move_in_timer = new Timer();

    [SerializeField] GameObject logo_group_end_pos = null;
    [SerializeField] float group_move_up_time = 1;

    [SerializeField] CanvasGroup play_button_group = null;
    [SerializeField] Button play_button = null;

    public override void UIBegin()
    {
        UIRestart();

        base.UIBegin();

        if (logo_image != null)
        {
            Canvas.ForceUpdateCanvases();

            logo_image.gameObject.SetActive(true);

            logo_image.transform.DOMove(new Vector3(logo_move_in_pos.transform.position.x,
                                                               logo_move_in_pos.transform.position.y,                                              transform.position.z), logo_move_in_time);
            orange_time_offset_move_in_timer.Start();
            blue_time_offset_in_timer.Start();
        }
    }

    public override void UIRestart()
    {
        state = MainMenuSelectState.FADING_IN;

        if (logo_image != null && logo_starting_pos != null)
        {
            logo_image.transform.position = new Vector3(logo_starting_pos.transform.position.x,
                                                             logo_starting_pos.transform.position.y,
                                                             gameObject.transform.position.z);
            logo_image.gameObject.SetActive(false);
        }

        if (orange_triangle != null && blue_triangle != null)
        {
            blue_triangle.transform.position = new Vector3(logo_image.transform.position.x,
                                                 blue_triangle.transform.position.y,
                                                 gameObject.transform.position.z);
            orange_triangle.transform.position = new Vector3(logo_image.transform.position.x,
                                                            orange_triangle.transform.position.y,
                                                            gameObject.transform.position.z);
            blue_triangle.SetActive(false);
            orange_triangle.SetActive(false);
        }

        if (play_button_group != null)
            play_button_group.alpha = 0.0f;

        if (play_button != null)
            play_button.gameObject.SetActive(false);
    }

    void Start ()
    {
      
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch(state)
        {
            case MainMenuSelectState.FADING_IN:
                {
                    if (blue_triangle != null)
                    {
                        if (blue_time_offset_in_timer.ReadTime() > blue_time_offset_move_in_time)
                        {
                            blue_triangle.gameObject.SetActive(true);
                            blue_triangle.transform.DOMove(new Vector3(logo_move_in_pos.transform.position.x,
                                blue_triangle.transform.position.y, transform.position.z), blue_triangle_move_in_time);

                            blue_triangle_move_in_timer.Start();
                        }
                    }

                    if (orange_triangle != null)
                    {
                        if (orange_time_offset_move_in_timer.ReadTime() > orange_time_offset_move_in_time)
                        {
                            orange_triangle.gameObject.SetActive(true);
                            orange_triangle.transform.DOMove(new Vector3(logo_move_in_pos.transform.position.x,
                                orange_triangle.transform.position.y, transform.position.z), orange_triangle_move_in_time);

                            orange_triangle_move_in_timer.Start();

                            state = MainMenuSelectState.TRIANGLES_FADING_IN;
                        }
                    }

                    break;
                }
            case MainMenuSelectState.TRIANGLES_FADING_IN:
                {
                    if(blue_triangle_move_in_timer.ReadTime() > blue_triangle_move_in_time)
                    {
                        if (orange_triangle_move_in_timer.ReadTime() > orange_triangle_move_in_time)
                        {
                            logo_group.transform.DOMove(logo_group_end_pos.transform.position, group_move_up_time);

                            if (play_button != null)
                            {
                                play_button.gameObject.SetActive(true);

                                play_button_group.DOFade(1, group_move_up_time);
                            }
                        }
                    }

                    break;
                }
        }
    }
}
