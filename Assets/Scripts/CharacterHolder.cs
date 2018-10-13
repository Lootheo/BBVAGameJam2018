using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour {
    public List<Item> legItems;
    public SpriteRenderer legPivot;
    public Item currentPants;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetNewItem(legItems[1]);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SetNewItem(legItems[0]);
        }
	}

    void SetNewItem(Item newItem)
    {
        switch (newItem.itemType)
        {
            case ItemType.HairStyle:
                break;
            case ItemType.HeadAccesory:
                break;
            case ItemType.Chest:
                break;
            case ItemType.Legs:
                legPivot.transform.position = newItem.itemOffset;
                legPivot.sprite = newItem.itemGraphic;
                break;
            case ItemType.Feet:
                break;
            default:
                break;
        }
    }
}
