using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocManager : Singleton<LocManager>
{
    public enum Language
    {
        EN,
        SPA,
        CAT,
    }

    public enum LanguagePart
    {
        MAIN_MENU,
        GAME_MENU,
        MAIN_GAME,
    }

    public class LanguageData
    {
        LanguageData(Language lan)
        {
            language = lan;
        }

        private Language language;

        private IDictionary<LanguagePart, LanguagePartData> content = new Dictionary<LanguagePart, LanguagePartData>();
    }

    public class LanguagePartData
    {
        LanguagePartData(Language lan, LanguagePart p)
        {
            language = lan;
            part = p;
        }

        private LanguagePart GetPart()
        {
            return part;
        }

        string GetContent(string key)
        {
            string ret = string.Empty;

            content.TryGetValue(key, out ret);

            if (string.IsNullOrEmpty(ret))
                ret = key + "[" + language.ToString() + "]" + "[" + part.ToString() + "]" + " No Text defined";
            
            return ret; 
        }

        private LanguagePart part;
        private Language language;

        private IDictionary<string, string> content = new Dictionary<string, string>();
    }

    private Language curr_language = Language.EN;

    private void Awake()
    {
        InitInstance(this, gameObject);
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
