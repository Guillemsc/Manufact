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
    [SerializeField] private float tiles_speed = 0.0f;

    [SerializeField] private Color base_move_tile_debug;
    [SerializeField] private Color base_static_tile_debug;

    [Header("Entities")]
    [SerializeField] private GameObject player = null;
    [SerializeField] private GameObject base_enemy = null;

    [SerializeField] private Color player_debug;
    [SerializeField] private Color base_enemy_debug;

    [Header("Bullets")]
    [SerializeField] private GameObject bullet_type_hit_move = null;
    [SerializeField] private GameObject bullet_ammo_hit_move = null;
    [SerializeField] private GameObject bullet_type_hit_static = null;
    [SerializeField] private GameObject bullet_ammo_hit_static = null;
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

    public float GetTilesMoveSpeed()
    {
        return tiles_speed;
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

    public GameObject GetBulletHitMove()
    {
        return bullet_type_hit_move;
    }

    public GameObject GetBulletAmmoHitMove()
    {
        return bullet_ammo_hit_move;
    }

    public GameObject GetBulletHitStatic()
    {
        return bullet_type_hit_static;
    }

    public GameObject GetBulletAmmoHitStatic()
    {
        return bullet_ammo_hit_static;
    }

    public float GetBulletsSpeed()
    {
        return bullets_speed;
    }
}
