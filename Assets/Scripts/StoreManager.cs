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

    public PlayerData avatarData;
	// Use this for initialization
	void Start () {
        avatarData = SaveData.Load();
        ShowItemsOfType(-1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowItemsOfType(int filter)
    {
        ClearShopObjects();
        List<Item> filteredList = new List<Item>();
        if (filter < 0)
        {
            filteredList = allItems;
        }
        else
        {
            ItemType type = (ItemType)filter;
            filteredList = allItems.Where(x => x.itemType == type).ToList();
        }
        FillShopItems(filteredList);
    }

    void FillShopItems(List<Item> shopItems)
    {
        foreach (Item item in shopItems)
        {
            GameObject itemInstance = Instantiate(shopEntryPrefab, scrollArea);
            ShopItem shopItem = itemInstance.GetComponent<ShopItem>();
            if (avatarData.avatarItems.Contains(item.itemID))
            {
                shopItem.itemBuyButton.interactable = false;
                shopItem.itemText.color = Color.red;
            }
            shopItem.itemImage.sprite = item.itemGraphic;
            shopItem.itemText.text = item.itemName;
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
