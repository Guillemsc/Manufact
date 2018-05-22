using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected void InitInstance(T _instance, GameObject go = null)
    {
        instance = _instance;

        object[] objects = FindObjectsOfType(typeof(T));

        if (objects.Length > 1)
        {
            Debug.LogError("[Singleton] Something went really wrong " +
                " - there should never be more than 1 singleton: " + typeof(T).ToString());
        }

        if (instance != null && go != null)
        {
            if (!Application.isEditor)
                DontDestroyOnLoad(go);
        }
    }

    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                Debug.LogError("[Singleton] Instance is not set, create it using 'InitInstance'");
                return null;
            }

        }
    }

    public static T instance;
}
