using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemGraphic;
    public ItemType itemType;
    public Vector2 itemOffset;

    public Item()
    {
        
    }

    public Item(Item _data)
    {
        itemName = _data.itemName;
        itemGraphic = _data.itemGraphic;
        itemType = _data.itemType;
        itemOffset = _data.itemOffset;
    }
}
