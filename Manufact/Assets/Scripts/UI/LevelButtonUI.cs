using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    private TMPro.TextMeshProUGUI level_number_text = null;

    RectTransform rt = null;

    private int stage = 0;
    private int level = 0;

    private void Awake()
    {
        rt = gameObject.GetComponent<RectTransform>();

        if (gameObject.transform.childCount > 0)
        {
            Transform child_trans = gameObject.transform.GetChild(0);

            if (child_trans != null)
            {
                level_number_text = child_trans.GetComponent<TMPro.TextMeshProUGUI>();
            }
        }

        Button butto = gameObject.GetComponent<Button>();

        if (butto != null)
            butto.onClick.AddListener(OnMouseDownIntern);
    }

    private void OnMouseDownIntern()
    {
        int i = 0;
    }

    public void SetLevel(int _stage, int _level)
    {
        if (level_number_text != null)
            level_number_text.text = _level.ToString();

        stage = _stage;
        level = _level;
    }

    public Vector2 GetSize()
    {
        Vector2 ret = new Vector2();

        ret = rt.rect.size;

        return ret;
    }

    public RectTransform GetRectTransform()
    {
        return rt;
    }
}
