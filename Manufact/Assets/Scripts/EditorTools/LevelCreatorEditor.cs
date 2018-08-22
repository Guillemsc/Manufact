using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelCreatorEditor : Singleton<LevelCreatorEditor>
{
    [Header("Tiles")]
    [SerializeField] private GameObject empty_tile = null;
    [SerializeField] private GameObject base_move_tile = null;
    [SerializeField] private GameObject base_static_tile = null;

    [SerializeField] private Color base_move_tile_debug;
    [SerializeField] private Color base_static_tile_debug;

    [Header("Entities")]
    [SerializeField] private GameObject player = null;
    [SerializeField] private GameObject base_enemy = null;

    [SerializeField] private Color player_debug;
    [SerializeField] private Color base_enemy_debug;

    [Header("Bullets")]
    [SerializeField] private GameObject base_bullet = null;
    [SerializeField] private GameObject base_bullet_ammo = null;
    [SerializeField] private float bullets_speed = 0.0f;

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

    public void NewPathInstance()
    {
        GameObject go = new GameObject();
        go.name = "PathInstance";
        go.AddComponent<EntityPathInstance>();
    }

    public GameObject GetEmptyTilePrefab()
    {
        return empty_tile;
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

    public GameObject GetPlayerPrefab()
    {
        return player;
    }

    public GameObject GetBaseEnemyPrefab()
    {
        return base_enemy;
    }

    public Color GetPlayerDebugColor()
    {
        return player_debug;
    }

    public Color GetBaseEnemyDebugColor()
    {
        return base_enemy_debug;
    }

    public Material GetLineMaterial()
    {
        return new Material(Shader.Find("Sprites/Default"));
    }

    public GameObject GetBaseBullet()
    {
        return base_bullet;
    }

    public GameObject GetBaseBulletAmmo()
    {
        return base_bullet_ammo;
    }

    public float GetBulletsSpeed()
    {
        return bullets_speed;
    }
}
