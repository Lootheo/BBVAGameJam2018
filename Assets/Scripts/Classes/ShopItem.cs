using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {
    public Item itemToBuy;
    public Image itemImage;
    public Text itemText;
    public Text itemPrice;
    public Button itemBuyButton;

    public void SetData(Item setItem, bool alreadyOwned)
    {
        itemToBuy = setItem;
        itemImage.sprite = itemToBuy.itemGraphic;
        itemText.text = itemToBuy.itemName;
        itemPrice.text = "$" + itemToBuy.itemPrice.ToString();
        if (alreadyOwned)
        {
            itemBuyButton.interactable = false;
            itemText.color = Color.red;
            itemPrice.gameObject.SetActive(false);
        }
    }
}

