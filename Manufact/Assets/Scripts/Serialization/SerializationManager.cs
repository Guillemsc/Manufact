using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializationManager : Singleton<SerializationManager>
{
    string language = "language";
    string level = "level";
    string first_time = "first_time";

    private void Awake()
    {
        InitInstance(this, gameObject);
    }

    public void ClearAllInfo()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SetFirstTime()
    {
        PlayerPrefs.SetInt(first_time, 0);
    }

    public bool GetFistTime()
    {
        bool ret = true;

        if (PlayerPrefs.HasKey(first_time))
        {
            bool value = PlayerPrefs.GetInt(first_time, 1) != 0;

            if (!value)
                ret = false;
        }

        return ret;
    }

    public bool GetHasLanguage()
    {
        bool ret = false;

        if (PlayerPrefs.HasKey(language))
            ret = true;

        return ret;
    }

    public void SetLanguage(LocManager.Language lan)
    {
        PlayerPrefs.SetInt(language, (int)lan);
    }

    public LocManager.Language GetLanguage()
    {
        LocManager.Language ret = new LocManager.Language();

        if(PlayerPrefs.HasKey(language))
        {
            int value = PlayerPrefs.GetInt(language);

            ret = (LocManager.Language)value;
        }

        return ret;
    }

    public void SetLevelsInfo()
    {
        
    }
}
