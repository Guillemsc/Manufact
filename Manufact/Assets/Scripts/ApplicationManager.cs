using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : Singleton<ApplicationManager> 
{
    [SerializeField] private bool is_release = false;

    [SerializeField] private string version = "0.1";

    private int frames_in_last_update = 0;
    private int frames_in_current_update = 0;
    private float curr_frame_time = 0.0f;

    private void Awake()
    {
        InitInstance(this, gameObject);
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

    private void Update()
    {
        UpdateFPS();
    }

    private void UpdateFPS()
    {
        curr_frame_time += Time.deltaTime;
        ++frames_in_current_update;

        if (curr_frame_time > 1)
        {
            frames_in_last_update = frames_in_current_update;
            curr_frame_time = 0;
            frames_in_current_update = 0;
        }
    }
}
