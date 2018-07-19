using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocText : MonoBehaviour
{
    [SerializeField] private string key;

    private Text text = null;
    private TextMeshProUGUI text_mp = null;

    private void Start()
    {
        text = gameObject.GetComponent<Text>();
        text_mp = gameObject.GetComponent<TextMeshProUGUI>();

        LocManager.Instance.AddUIText(this);

        SetText();
    }

    public void SetText()
    {
        if(text != null)
            text.text = LocManager.Instance.GetText(key);

        if(text_mp != null)
            text_mp.text = LocManager.Instance.GetText(key);
    }

    private void OnDestroy()
    {
        LocManager.Instance.RemoveUIText(this);
    }
}
