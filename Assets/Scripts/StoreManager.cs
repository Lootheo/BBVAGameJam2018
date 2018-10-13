using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StoreManager : MonoBehaviour {
    public GameObject shopEntryPrefab;
    public RectTransform scrollArea;
    public List<Item> allItems;
    public List<GameObject> currentShopItems;
    public ItemType itemTypeFilter;
    public PlayerData avatarData;
	// Use this for initialization
	void Start () {
        avatarData = SaveData.Load();
        ShowItemsOfType(-1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeFilter(bool currentValue)
    {
        switch (itemTypeFilter)
        {
            case ItemType.Cloth:
                itemTypeFilter = ItemType.Furniture;
                break;
            case ItemType.Furniture:
                itemTypeFilter = ItemType.Cloth;
                break;
            default:
                break;
        }
        ShowItemsOfType(-1);
    }

    public void ShowItemsOfType(int filter)
    {
        ClearShopObjects();
        List<Item> filteredList = new List<Item>();
        if (filter < 0)
        {
            filteredList = allItems.Where(x => x.itemType == itemTypeFilter).ToList();
        }
        else if (filter > 0)
        {
            switch (itemTypeFilter)
            {
                case ItemType.Cloth:
                    filteredList = allItems.Where(x => x.clothType == (ClothType)filter).ToList();
                    break;
                case ItemType.Furniture:
                    filteredList = allItems.Where(x => x.furnitureType == (FurnitureType)filter).ToList();
                    break;
                default:
                    break;
            }
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
            
            scrollArea.sizeDelta += new Vector2(320, 0);
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
}
