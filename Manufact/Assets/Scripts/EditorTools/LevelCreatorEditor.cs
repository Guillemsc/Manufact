using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelCreatorEditor : MonoBehaviour
{
    public void NewGridInstance()
    {
        GameObject go = new GameObject();
        go.name = "GridInstance";
        go.AddComponent<GridInstance>();
    }
}
