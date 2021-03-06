﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConstructionManager : MonoBehaviour {
    public GameObject shopEntryPrefab;
    public RectTransform scrollArea;
    public List<Item> ownedFurniture;
    public List<GameObject> currentShopItems;
    public RoomManager rm;
    public DataSender sender;
    public Canvas dragCanvas;
    public GameObject furniturePrefab;
    public GameObject goldGeneratorPrefab;
    List<GameObject> roomFurniture = new List<GameObject>();
    public SpriteRenderer roomSprite;
    public List<GameObject> goldGenerators = new List<GameObject>();
    // Use this for initialization

    public static ConstructionManager instance;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
        ConstructRoom();
    }

    void ConstructRoom()
    {
        //Item currentRoom = rm.allItems.Find(x => x.itemType == ItemType.Room && x.displayed == true);

        foreach (Furniture item in rm.avatarData.houseItems)
        {
            Item owned = rm.allItems.Find(x => x.itemID == item.furnitureID && item.displayed == true);
            if (owned != null)
            {
                if (owned.itemType == ItemType.Room)
                {
                    roomSprite.sprite = owned.itemGraphic;
                    continue;
                }
                GameObject itemInstance = Instantiate(furniturePrefab, new Vector3(item.positionX, item.positionY, -5f), Quaternion.identity) as GameObject;
                GameObject goldGenerator = Instantiate(goldGeneratorPrefab, new Vector3(item.positionX, item.positionY, -7f), Quaternion.identity,GameObject.Find("Canvas").transform) as GameObject;
                goldGenerators.Add(goldGenerator);
                itemInstance.GetComponent<SpriteRenderer>().sprite = owned.itemGraphic;
                itemInstance.AddComponent<PolygonCollider2D>();
                roomFurniture.Add(itemInstance);
            }
        }
    }

    public void ShowItemsOfType()
    {
        ClearShopObjects();
        List<Item> filteredList = new List<Item>();
        foreach (Furniture item in rm.avatarData.houseItems)
        {
            Item owned = rm.allItems.Find(x => x.itemID == item.furnitureID && item.displayed == false);
            if (owned!=null)
            {
                Debug.Log(owned.itemName + " " + owned.itemID);
                filteredList.Add(owned);
            }
        }
        FillShopItems(filteredList);
    }

    void FillShopItems(List<Item> shopItems)
    {
        foreach (Item item in shopItems)
        {
            GameObject itemInstance = Instantiate(shopEntryPrefab, scrollArea);
            DragBuild shopItem = itemInstance.GetComponent<DragBuild>();
            shopItem.SetData(sender, dragCanvas, item);
            scrollArea.sizeDelta += new Vector2(80, 0);
            currentShopItems.Add(itemInstance);
        }
    }

    public void ChangeRoom(Item newRoom)
    {
        roomSprite.sprite = newRoom.itemGraphic;
        Furniture furniture = rm.avatarData.houseItems.Find(x => x.furnitureID == newRoom.itemID);
        furniture.displayed = true;
        int dex = rm.avatarData.houseItems.IndexOf(furniture);
        rm.avatarData.houseItems[dex].displayed = true;
        rm.avatarData.SetPlayerAccountData(PlayerAccountManager.instance);
        
        SaveData.Save(rm.avatarData);
        GameObject.FindObjectOfType<FinalFirebaseConnection>().WriteNewUserData(rm.avatarData.avatarName, new ServerData(rm.avatarData));
    }

    void ClearShopObjects()
    {
        foreach (GameObject item in currentShopItems)
        {
            Destroy(item);
        }
        foreach(GameObject goldGenerator in goldGenerators)
        {
            Destroy(goldGenerator);
        }
        currentShopItems.Clear();
        scrollArea.sizeDelta = new Vector2(0, scrollArea.sizeDelta.y);
    }

    public void ClearRoom()
    {
        foreach (GameObject item in roomFurniture)
        {
            Destroy(item);
        }
        foreach (Furniture item in rm.avatarData.houseItems)
        {
            item.displayed = false;
        }
        ShowItemsOfType();
    }

    public void BuildItemAtPosition(Item item, Vector3 pos)
    {
        GameObject itemInstance = Instantiate(furniturePrefab, pos, Quaternion.identity) as GameObject;
        GameObject goldGenerator = Instantiate(goldGeneratorPrefab, pos, Quaternion.identity) as GameObject;
        itemInstance.GetComponent<SpriteRenderer>().sprite = item.itemGraphic;
        itemInstance.AddComponent<PolygonCollider2D>();
        roomFurniture.Add(itemInstance);
        Furniture furniture = rm.avatarData.houseItems.Find(x => x.furnitureID == item.itemID);
        
        furniture.displayed = true;
        int dex = rm.avatarData.houseItems.IndexOf(furniture);
        rm.avatarData.houseItems[dex].displayed = true;
        rm.avatarData.houseItems[dex].positionX = pos.x;
        rm.avatarData.houseItems[dex].positionY = pos.y;
        ShowItemsOfType();
        rm.avatarData.SetPlayerAccountData(PlayerAccountManager.instance);
        SaveData.Save(rm.avatarData);
        GameObject.FindObjectOfType<FinalFirebaseConnection>().WriteNewUserData(rm.avatarData.avatarName, new ServerData(rm.avatarData));
    }
}
