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
            ShopItem shopItem = itemInstance.GetComponent<ShopItem>();
            shopItem.SetData(item, avatarData.avatarItems.Contains(item.itemID));

            scrollArea.sizeDelta += new Vector2(0, 160);
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
        scrollArea.sizeDelta = new Vector2(scrollArea.sizeDelta.x, 0);
    }
}
