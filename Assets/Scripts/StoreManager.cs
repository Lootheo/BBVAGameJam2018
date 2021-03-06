﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StoreManager : MonoBehaviour {
    public GameObject shopEntryPrefab;
    public RectTransform scrollArea;
    public List<GameObject> currentShopItems;
    public ItemType itemTypeFilter;
    public UI_Dialog confirmationDialog;
    public RoomManager rm;
    public GameObject storeWindow;

    // Use this for initialization
    void Start () {
        ShowItemsOfType(1);
	}

    public static StoreManager instance;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }

    public bool CanBuyWithPoints(int price)
    {
        return price <= PlayerAccountManager.instance.Gold;
    }

    public bool CanBuyWithCredit(int price)
    {
        return price <= PlayerAccountManager.instance.CreditAccount.AvailableCredit;
    }

    public void BuyWithCredit(Item item)
    {
        rm.avatarData.purchasedItems.Add(item.itemID);
        

        if (item.itemType == ItemType.Furniture || item.itemType == ItemType.Room)
        {
            rm.avatarData.houseItems.Add(new Furniture(item.itemID, 0, 0, false));
        }
        PlayerAccountManager.instance.BuyWithCredit(item);
        rm.avatarData.SetPlayerAccountData(PlayerAccountManager.instance);
        SaveData.Save(rm.avatarData);
        
        confirmationDialog.gameObject.SetActive(false);
    }

    public void BuyWithPoints(Item item)
    {
        rm.avatarData.purchasedItems.Add(item.itemID);


        if (item.itemType == ItemType.Furniture || item.itemType == ItemType.Room)
        {
            rm.avatarData.houseItems.Add(new Furniture(item.itemID, 0, 0, false));
        }
        PlayerAccountManager.instance.BuyWithGold(item);
        rm.avatarData.SetPlayerAccountData(PlayerAccountManager.instance);
        SaveData.Save(rm.avatarData);

        confirmationDialog.gameObject.SetActive(false);
        storeWindow.gameObject.SetActive(false);
    }

    public void ChangeFilter(int type)
    {
        switch (type)
        {
            case 0:
                itemTypeFilter = ItemType.Furniture;
                break;
            case 1:
                itemTypeFilter = ItemType.Cloth;
                break;
            case 2:
                itemTypeFilter = ItemType.Room;
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
            filteredList = rm.allItems.Where(x => x.itemType == itemTypeFilter).ToList();
        }
        else if (filter > 0)
        {
            switch (itemTypeFilter)
            {
                case ItemType.Cloth:
                    filteredList = rm.allItems.Where(x => x.clothType == (ClothType)filter).ToList();
                    break;
                case ItemType.Furniture:
                    filteredList = rm.allItems.Where(x => x.furnitureType == (FurnitureType)filter).ToList();
                    break;
                case ItemType.Room:
                    filteredList = rm.allItems.Where(x => x.itemType == ItemType.Room).ToList();
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
            shopItem.SetData(item, rm.avatarData.avatarItems.Contains(item.itemID));
            shopItem.itemBuyButton.onClick.AddListener(() => CheckMoneyToBuy(shopItem.itemToBuy));
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

    public void CheckMoneyToBuy(Item item)
    {
        if (CanBuyWithPoints(item.itemPrice))
        {
            //OpenBuyConfirmationWindow(item);
            BuyWithPoints(item);
        }
        else
        {
            OpenDeniedShopingWindow(item);
        }
    }

    public void OpenBuyConfirmationWindow(Item item)
    {
        confirmationDialog.gameObject.SetActive(true);
        confirmationDialog.warningText.text = "El objeto " + item.name + " tiene un costo de " + item.itemPrice + ".00MX y tienes un credito disponible de"
            + PlayerAccountManager.instance.Gold + ".00MX. Presiona Aceptar para hacer un nuevo prestamo.";
        confirmationDialog.confirmButton.onClick.AddListener(() => BuyWithPoints(item));
        confirmationDialog.confirmButton.onClick.AddListener(CloseWindow);

    }

    public void OpenDeniedShopingWindow(Item item)
    {
        confirmationDialog.gameObject.SetActive(true);
        confirmationDialog.warningText.text = "No tienes suficiente saldo para comprar el objeto " + item.itemName + ". Saldo disponible: " +
            PlayerAccountManager.instance.Gold + ".00MX. Presiona Aceptar para hacer un nuevo prestamo.";
        confirmationDialog.confirmButton.onClick.AddListener(CloseWindow);
        confirmationDialog.confirmButton.onClick.AddListener(askmoney);
    }

    public void askmoney()
    {
        PlayerAccountManager.instance.AskEffectiveCredit(5000);
    }

    public void CloseWindow()
    {
        confirmationDialog.gameObject.SetActive(false);
        
    }
}
