using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocText : MonoBehaviour
{
    [SerializeField] private string key;

    private Text text = null;

    private void Start()
    {
        text = gameObject.GetComponent<Text>();
        LocManager.Instance.AddUIText(this);

        SetText();
    }

    public void SetText()
    {
        if(text != null)
            text.text = LocManager.Instance.GetText(key);
    }

    private void OnDestroy()
    {
        LocManager.Instance.RemoveUIText(this);
    }
}
