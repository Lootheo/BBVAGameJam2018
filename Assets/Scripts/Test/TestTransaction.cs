using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestTransaction : MonoBehaviour {

    
    public Item testItem;
    public int paydebt;
    public int pointstoadd;
    public int interest;
    public int creditLimit;
    public void setCredit()
    {
        PlayerAccountManager.instance.SetCreditAccount(0, DateTime.Now.ToString(), new Credit(interest, creditLimit));
    }

    public void AddPoints()
    {
        PlayerAccountManager.instance.AddPlayerPoints(pointstoadd);
    }

    public void PayDebt()
    {
        PlayerAccountManager.instance.PayCredit(paydebt);
    }

    public void SetTransactionWithCredit()
    {  
        PlayerAccountManager.instance.BuyWithCredit(testItem);
    }

    public void SetTransactionWithPoints()
    {
        PlayerAccountManager.instance.BuyWithPoints(testItem);
    }

    
}
