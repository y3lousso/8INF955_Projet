﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereManager : MonoBehaviour {

    public static AtmosphereManager Instance;
    public List<MaterialSwapper> materialSwappers;

    public Material redMaterial;
    public Material blueMaterial;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can't have multiple atmosphere manager.");
        }
    }

    // Use this for initialization
    void Start () {
        materialSwappers = new List<MaterialSwapper>();
        materialSwappers.AddRange(FindObjectsOfType<MaterialSwapper>());
    }
	
	public void SetAmbiance(CubeColor color)
    {
        if(color == CubeColor.Blue)
        {
            foreach(MaterialSwapper m in materialSwappers)
            {
                m.SetMaterial(blueMaterial);
            }              
        }
        else
        {
            foreach (MaterialSwapper m in materialSwappers)
            {
                m.SetMaterial(redMaterial);
            }
        }
    }


}