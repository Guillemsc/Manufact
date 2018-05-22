using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class LocManager : Singleton<LocManager>
{
    [SerializeField] private string localization_file_name;

    public enum Language
    {
        EN,
        SPA,
        CAT,
    }

    private IDictionary<string, string> _content = new Dictionary<string, string>();

    private Language language = Language.EN;

    private List<LocText> texts = new List<LocText>();

    public void Awake()
    {
        InitInstance(this, gameObject);

        SetLanguage(Language.CAT);
    }

    public void SetLanguage(Language _language)
    {
        language = _language;

        LoadData(_language);

        for (int i = 0; i < texts.Count; ++i)
        {
            texts[i].SetText();
        }
    }

    public Language GetLanguage()
    {
        return language;
    }

    public string GetText(string key)
    {
        string ret = string.Empty;

        _content.TryGetValue(key, out ret);

        if (string.IsNullOrEmpty(ret))
            ret = key + "[" + language.ToString() + "]" + " No Text defined";

        return ret;
    }

    public void AddUIText(LocText text)
    {
        texts.Add(text);
    }

    public void RemoveUIText(LocText text)
    {
        texts.Remove(text);
    }

    private void LoadData(Language language)
    {
        _content.Clear();

        XmlDocument xml_document = new XmlDocument();

        TextAsset text_asset = (TextAsset)Resources.Load(localization_file_name);

        if(text_asset == null)
        {
            Debug.LogError("[XML] Couldnt Load Xml");
            return;
        }

        string path = text_asset.text;
        xml_document.LoadXml(path);

        if (xml_document == null)
        {
            Debug.LogError("[XML] Couldnt Load Xml: " + xml_document.Name);
            return;
        }

        XmlNode xNode = xml_document.ChildNodes.Item(1).ChildNodes.Item(0);

        foreach (XmlNode node in xNode.ChildNodes)
        {
            if (node.LocalName == "TextKey")
            {
                string value = node.Attributes.GetNamedItem("name").Value;
                string text = string.Empty;
                foreach (XmlNode langNode in node)
                {
                    if (langNode.LocalName == language.ToString())
                    {
                        text = langNode.InnerText;

                        if (_content.ContainsKey(value))
                        {
                            _content.Remove(value);
                            _content.Add(value, value + " has been found multiple times in the XML allowed only once!");
                        }
                        else
                        {
                            _content.Add(value, (!string.IsNullOrEmpty(text)) ? text : ("No Text for " + value + " found"));
                        }
                        break;
                    }
                }
            }
        }
    }
}