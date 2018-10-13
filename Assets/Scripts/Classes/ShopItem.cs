using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopItem : MonoBehaviour {
    public Item itemToBuy;
    public Image itemImage;
    public Text itemText;
    public Text itemPrice;
    public Button itemBuyButton;
    public TextMeshProUGUI canBuyText;

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
            canBuyText.gameObject.SetActive(false);
            itemPrice.gameObject.SetActive(false);
        }
    }
}

