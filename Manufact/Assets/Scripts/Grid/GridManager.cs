using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public class Grid
    {
        public Grid()
        {

        }

        private List<GridSlot> grid_slots = new List<GridSlot>();
    }

    public class GridSlot
    {
        public GridSlot()
        {

        }

        private GameObject game_object = null;
        private SpriteRenderer sprite_renderer = null;
        private Collider2D collider = null;
        private float grid_size = 0.0f;
    }

    private void Awake()
    {
        InitInstance(this, gameObject);
    }

    private void Start ()
    {
		
	}

    private void Update () {
		
	}
}
