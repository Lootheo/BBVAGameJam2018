﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ClickableFurniture : MonoBehaviour {

    //Furniture data
    public string Name;
    public string description;
    public int price;
    public bool purchased;
    public Sprite image;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!purchased)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.3f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
