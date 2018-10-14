using System.Collections;
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
        rm.avatarData.houseItems.Add(new Furniture(item.itemID, Vector2.zero, false));
        SaveData.Save(rm.avatarData);
        PlayerAccountManager.instance.BuyWithCredit(item);
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
            OpenBuyConfirmationWindow(item);
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
            + PlayerAccountManager.instance.CreditAccount.AvailableCredit + ".00MX. Presiona Aceptar para confirmar la compra.";
        confirmationDialog.confirmButton.onClick.AddListener(() => BuyWithCredit(item));
    }

    public void OpenDeniedShopingWindow(Item item)
    {
        confirmationDialog.gameObject.SetActive(true);
        confirmationDialog.warningText.text = "No tienes suficiente credito para comprar el objeto " + item.name + ". Credito disponible: " +
            PlayerAccountManager.instance.CreditAccount.AvailableCredit + ".00MX. Te sugerimos saldar tu cuenta para comprar este objeto.";
        confirmationDialog.confirmButton.onClick.AddListener(CloseWindow);
    }

    public void CloseWindow()
    {
        confirmationDialog.gameObject.SetActive(false);
    }
}
