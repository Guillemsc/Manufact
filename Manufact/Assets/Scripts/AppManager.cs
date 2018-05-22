using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : Singleton<AppManager> 
{
    [SerializeField] private bool is_release = false;

    [SerializeField] private string version = "0.1";

    [SerializeField] private bool vsync = true;

    [SerializeField] private int max_fps = 60;

    private int frames_in_last_update = 0;
    private int frames_in_current_update = 0;
    Timer fps_timer = new Timer();

    private void Awake()
    {
        InitInstance(this, gameObject);
    }

    private void Start()
    {
        fps_timer.Start();

        SetVSync(vsync);
        SetMaxFPS(max_fps);
    }

    private void Update()
    {
        UpdateFPS();
    }

    public bool GetIsRelease()
    {
        return is_release;
    }

    public string GetVersion()
    {
        return version;
    }

    public int GetFPS()
    {
        return frames_in_last_update;
    }

    public void SetVSync(bool set)
    {
        vsync = set;
        QualitySettings.vSyncCount = (set == true ? 1 : 0);
    }

    public bool GetVSync()
    {
        return QualitySettings.vSyncCount == 1;
    }

    public void SetMaxFPS(int set)
    {
        if (max_fps > 0)
        {
            max_fps = set;
            Application.targetFrameRate = set;
        }
    }

    public int GetMaxFPS()
    {
        return Application.targetFrameRate;
    }

    private void UpdateFPS()
    {
        ++frames_in_current_update;

        if (fps_timer.ReadFixedTime() > 1)
        {
            frames_in_last_update = frames_in_current_update;
            frames_in_current_update = 0;

            fps_timer.Start();
        }
    }
}
