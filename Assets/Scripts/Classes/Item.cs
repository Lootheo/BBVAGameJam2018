using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class Item : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemGraphic;
    public ItemType itemType;
    public ClothType clothType;
    public FurnitureType furnitureType;
    public Vector2 itemOffset;
    public int itemPrice;
    public string Description;
    public Item()
    {
        
    }

    public Item(Item _data)
    {
        itemID = _data.itemID;
        itemName = _data.itemName;
        itemGraphic = _data.itemGraphic;
        itemType = _data.itemType;
        clothType = _data.clothType;
        furnitureType = _data.furnitureType;
        itemOffset = _data.itemOffset;
        itemPrice = _data.itemPrice;
    }
}