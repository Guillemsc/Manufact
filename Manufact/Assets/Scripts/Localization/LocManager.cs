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
    }

    private IDictionary<string, string> _content = new Dictionary<string, string>();

    private Language language = Language.EN;

    private List<LocText> texts = new List<LocText>();

    public void Awake()
    {
        InitInstance(this, gameObject);

        SetLanguage(Language.EN);
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

    public string GetText(string key, object[] values = null)
    {
        string ret = string.Empty;

        _content.TryGetValue(key, out ret);

        if (values != null)
        {
            if (string.IsNullOrEmpty(ret))
                ret = key + "[" + language.ToString() + "]" + " No Text defined";
            else
            {
                ret = SubstituteValues(ret, values);
            }
        }

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

    private string SubstituteValues(string text, object[] values)
    {
        string ret = "";

        string val_to_check = "%value%";
        int index_val_to_check = 0;

        int curr_value = 0;

        string to_check = "";
        
        for(int i = 0; i < text.Length; ++i)
        {            
            if (values.Length > curr_value && text[i] == val_to_check[index_val_to_check])
            {
                ++index_val_to_check;

                to_check += text[i];

                if (index_val_to_check >= val_to_check.Length)
                {
                    ret += values[curr_value].ToString();

                    to_check = "";

                    ++curr_value;
                }
            }
            else
            {
                index_val_to_check = 0;

                ret += to_check + text[i];
                to_check = "";
            }
        }

        return ret;
    }
}