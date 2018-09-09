using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonBreathUI : MonoBehaviour
{
    [SerializeField] Image breath_image = null;
    float starting_breath_scale = 0.0f;

    Timer breath_wait_timer = new Timer();
    float breath_wait_time = 0.7f;
    bool waiting_breath = false;

    Timer breath_timer = new Timer();
    float breath_time = 2.0f;
    bool  breath = false;

    // Use this for initialization
    void Start ()
    {
        breath = false;
        waiting_breath = true;
        breath_wait_timer.Start();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(waiting_breath)
        {
            if(breath_wait_timer.ReadTime() > breath_wait_time)
            {
                breath = true;
                waiting_breath = false;
                breath_timer.Start();

                starting_breath_scale = breath_image.transform.localScale.x;

                breath_image.DOFade(1.0f, 0.0f);
                breath_image.transform.DOScale(starting_breath_scale + 0.4f, breath_time);
                breath_image.DOFade(0.0f, breath_time);
            }
        }

        if (breath)
        {
            if (breath_timer.ReadTime() > breath_time)
            {
                breath = false;
                waiting_breath = true;
                breath_wait_timer.Start();

                breath_image.transform.DOScale(starting_breath_scale, 0.1f);
            }
        }

    }
}
