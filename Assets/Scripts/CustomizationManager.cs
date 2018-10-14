using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour {
    public RectTransform gridRect;
    public GameObject itemPrefab;
    public CharacterHolder holder;
    public DataSender sender;
    public Canvas mainCanvas;
    public Text nameField;
    public RoomManager rm;

    public List<Item> ownedClothes = new List<Item>();

    List<DragMe> entries = new List<DragMe>();

    // Use this for initialization
    void Start()
    {
        foreach (int itemID in rm.avatarData.purchasedItems)
        {
            Item purchasedItem = rm.allItems.Find(x => x.itemID == itemID);
            ownedClothes.Add(purchasedItem);
        }
        ownedClothes = ownedClothes.Where(x => x.itemType == ItemType.Cloth).ToList();
        SetCharacterLook();
        FillGrid(1);
    }

    void SetCharacterLook()
    {
        foreach (int itemID in rm.avatarData.avatarItems)
        {
            Item purchasedItem = ownedClothes.Find(x => x.itemID == itemID);
            if (purchasedItem!=null)
            {
                holder.SetItemPiece(purchasedItem);
            }  
        }
    }

    public void FillGrid(int itemType)
    {
        List<Item> filteredList = new List<Item>();
        foreach (Item item in ownedClothes)
        {
            if (item.clothType == (ClothType)itemType)
            {
                filteredList.Add(item);
            }
        }
        FillShopItems(filteredList);
    }

    void FillShopItems(List<Item> shopItems)
    {
        ClearGrid();
        foreach (Item item in shopItems)
        {
            GameObject itemInstance = Instantiate(itemPrefab, gridRect);
            DragMe shopItem = itemInstance.GetComponent<DragMe>();
            shopItem.SetData(sender, mainCanvas, item);
            shopItem.icon.sprite = item.itemGraphic;
            entries.Add(shopItem);
        }
    }

    void ClearGrid()
    {
        foreach (DragMe item in entries)
        {
            Destroy(item.gameObject);
        }
        entries.Clear();
    }
}
