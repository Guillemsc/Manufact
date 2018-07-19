using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuUI : UIControl
{
    [SerializeField] private Image logo_image = null;
    [SerializeField] private GameObject logo_end_pos = null;

    private void Awake()
    {
        if (logo_image != null)
        {
            logo_image.transform.localPosition = new Vector3(logo_image.transform.localPosition.x + 1000,
                                                             logo_image.transform.localPosition.y,
                                                             gameObject.transform.position.z);
            logo_image.gameObject.SetActive(false);
        }
    }

    public override void UIBegin()
    {
        base.UIBegin();

        if (logo_image != null)
        {
            logo_image.transform.DOMove(new Vector3(logo_end_pos.transform.position.x, 
                                                               logo_end_pos.transform.position.y, 
                                                               transform.position.z), 1);

            logo_image.gameObject.SetActive(true);
        }
    }

    void Start ()
    {
        UIBegin();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
