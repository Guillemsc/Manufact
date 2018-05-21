using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected void InitInstance(T _instance, GameObject go = null)
    {
        Instance = _instance;

        object[] objects = FindObjectsOfType(typeof(T));

        if (objects.Length > 1)
        {
            Debug.LogError("[Singleton] Something went really wrong " +
                " - there should never be more than 1 singleton: " + typeof(T).ToString());
        }

        if (Instance != null && go != null)
        {
            DontDestroyOnLoad(go);
        }
    }

    public static T Instance;
}
