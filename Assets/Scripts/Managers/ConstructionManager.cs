using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConstructionManager : MonoBehaviour {
    public GameObject shopEntryPrefab;
    public RectTransform scrollArea;
    public List<Item> ownedFurniture;
    public List<GameObject> currentShopItems;
    public PlayerData avatarData;
    public List<Item> allItems;
    public DataSender sender;
    public Canvas dragCanvas;
    public GameObject furniturePrefab;
    // Use this for initialization
    void Start()
    {
        avatarData = SaveData.Load();
        foreach (Furniture item in avatarData.houseItems)
        {
            Item owned = allItems.Find(x => x.itemID == item.furnitureID);
            ownedFurniture.Add(owned);
        }
        ShowItemsOfType(-1);
    }

    public static ConstructionManager instance;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }

    public void ShowItemsOfType(int filter)
    {
        ClearShopObjects();
        List<Item> filteredList = new List<Item>();
        if (filter < 0)
        {
            filteredList = ownedFurniture;
        }
        else if (filter > 0)
        {
            filteredList = allItems.Where(x => x.furnitureType == (FurnitureType)filter).ToList();
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
            //Text itemText = shopItem.
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

    public void BuildItemAtPosition(Item item, Vector3 pos)
    {
        GameObject itemInstance = Instantiate(furniturePrefab, pos, Quaternion.identity) as GameObject;
        itemInstance.GetComponent<SpriteRenderer>().sprite = item.itemGraphic;
        itemInstance.AddComponent<PolygonCollider2D>();
    }
}
