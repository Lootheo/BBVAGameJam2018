using System.Collections;
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
    List<GameObject> roomFurniture = new List<GameObject>();
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
        foreach (Furniture item in rm.avatarData.houseItems)
        {
            Item owned = rm.allItems.Find(x => x.itemID == item.furnitureID && item.displayed == true);
            if (owned != null)
            {
                GameObject itemInstance = Instantiate(furniturePrefab, new Vector3(item.positionX, item.positionY, -5f), Quaternion.identity) as GameObject;
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

    void ClearShopObjects()
    {
        foreach (GameObject item in currentShopItems)
        {
            Destroy(item);
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
    }
}
