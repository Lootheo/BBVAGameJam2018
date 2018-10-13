using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingManager : MonoBehaviour {

    public static ShoppingManager instance;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }

    public bool CanBuyWithPoints(int price)
    {
        return price <= PlayerAccountManager.instance.PlayerPoints;
    }

    public bool CanBuyWithCredit(int price)
    {
        return price <= PlayerAccountManager.instance.CreditAccount.AvailableCredit;
    }
}
