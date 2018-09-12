using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : Singleton<GooglePlayManager>
{
    private void Awake()
    {
        InitInstance(this, gameObject);

        PlayGamesPlatform.Activate();
    }

    public bool LogIn()
    {
        bool ret = false;

        Social.localUser.Authenticate((bool succes) =>
        {
            if (succes)
                ret = true;
        });

        return ret;
    }
}
