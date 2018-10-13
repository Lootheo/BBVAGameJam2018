using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData{
    public List<int> avatarItems;
    public string avatarName;

    public PlayerData()
    {
        avatarItems = new List<int>();
        avatarName = "";
    }

    public PlayerData(List<int> items, string name)
    {
        avatarItems = items;
        avatarName = name;
    }
}
