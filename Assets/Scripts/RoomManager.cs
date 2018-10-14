using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
    private static RoomManager instance = null;
    public GameObject conceptualUI;
    public PlayerData avatarData;
    public List<Item> allItems;
    public static RoomManager GetInstance()
    {
        if (instance == null)
        {
            instance = new RoomManager();
        }

        return instance;
    }

    private void Awake()
    {
        instance = this;
        avatarData = SaveData.Load();
    }

    private void Start()
    {
        conceptualUI.SetActive(false);
    }

    public void ShowConceptualMenu(Vector2 playerPos, bool show)
    {
        conceptualUI.transform.position = playerPos;
        conceptualUI.SetActive(show);
    }
}
