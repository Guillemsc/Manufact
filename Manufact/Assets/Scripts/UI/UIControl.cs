using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public virtual void UIBegin()
    {
        gameObject.SetActive(true);
    }

    public virtual void UIRestart()
    {

    }

    public void UISuscribeOnFinish(UIFinish finish)
    {
        on_finish -= finish;
        on_finish += finish;
    }

    protected void UIOnFinish()
    {
        if(on_finish != null)
            on_finish(this);

        if (disable_on_finish)
            gameObject.SetActive(false);
    }

    public void SetDisableOnFinish(bool set)
    {
        disable_on_finish = set;
    }

    public delegate void UIFinish(UIControl con);

    private event UIFinish on_finish = null;

    private bool disable_on_finish = false;
}
