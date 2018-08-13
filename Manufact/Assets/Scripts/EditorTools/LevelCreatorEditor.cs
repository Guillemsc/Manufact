using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelCreatorEditor : Singleton<LevelCreatorEditor>
{
    [SerializeField] private GameObject base_move_tile = null;
    [SerializeField] private GameObject base_static_tile = null;

    [SerializeField] private Color base_move_tile_debug;
    [SerializeField] private Color base_static_tile_debug;

    private void Awake()
    {
        InitInstance(this, gameObject);
    }

    private void Start()
    {
        InitInstance(this, gameObject);
    }

    private void Update()
    {
        if(!Application.isPlaying)
            InitInstance(this, gameObject);
    }

    public void NewGridInstance()
    {
        GameObject go = new GameObject();
        go.name = "GridInstance";
        go.AddComponent<GameGridInstance>();
    }

    public GameObject GetBaseMoveTilePrefab()
    {
        return base_move_tile;
    }

    public GameObject GetStaticTilePrefab()
    {
        return base_static_tile;
    }

    public Color GetBaseMoveTileDebugColor()
    {
        return base_move_tile_debug;
    }

    public Color GetStaticTileDebugColor()
    {
        return base_static_tile_debug;
    }
}
