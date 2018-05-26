using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridCreatorManager : Singleton<GridCreatorManager>
{
     private void Awake()
    {
        InitInstance(this, gameObject);
    }
    
    public void CreateGridInstance()
    {
        GameObject go = new GameObject();
        go.AddComponent<GridInstanceEditor>();
        go.AddComponent<GridInstance>();
        go.name = "GridInstance";
    }
}
