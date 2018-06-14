using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialEntity : MonoBehaviour
{
    public enum MaterialEntityType
    {
        MATERIAL_WOOD
    }

    [SerializeField]
    private MaterialEntityType material_type;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public MaterialEntityType GetType()
    {
        return material_type;
    }
}
