using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuUI : UIControl
{
    [SerializeField] private Image logo_image = null;

    [SerializeField] private GameObject logo_starting_pos = null;
    [SerializeField] private GameObject logo_move_in_pos = null;
    [SerializeField] private GameObject logo_move_out_pos = null;

    [SerializeField] private float logo_move_in_time = 1.0f;
    [SerializeField] private float logo_move_out_time = 1.0f;

    private void Awake()
    {
        if (logo_image != null && logo_starting_pos != null)
        {
            logo_image.transform.position = new Vector3(logo_starting_pos.transform.position.x,
                                                             logo_starting_pos.transform.position.y,
                                                             gameObject.transform.position.z);
            logo_image.gameObject.SetActive(false);
        }
    }

    public override void UIBegin()
    {
        base.UIBegin();

        if (logo_image != null)
        {
            logo_image.gameObject.SetActive(true);

            logo_image.transform.DOMove(new Vector3(logo_move_in_pos.transform.position.x,
                                                               logo_move_in_pos.transform.position.y, 
                                                               transform.position.z), logo_move_in_time);
        }
    }

    void Start ()
    {
      
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
