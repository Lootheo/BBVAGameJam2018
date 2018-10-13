using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ClickableFurniture : ClickableItem {

    //Furniture data
    public string Name;
    public string description;
    public int price;
    public bool purchased;
    public Sprite image;
	// Update is called once per frame
	void Update () {
        /*if (!purchased)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.3f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }*/
    }

    public override void OnItemClick()
    {
        base.OnItemClick();
    }
}
