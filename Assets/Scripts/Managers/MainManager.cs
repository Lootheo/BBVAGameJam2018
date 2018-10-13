using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainManager : MonoBehaviour {



    private void Start()
    {
        DateTime cutDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
        PlayerData data = SaveData.Load();
        Credit credit = new Credit(data.accountData.InterestRate, data.accountData.CreditLimit);
        if (DateTime.Now.Hour >= 6)
        {
            TimeSpan span = new TimeSpan(24, 0, 0);
            cutDateTime.Add(span);
            Debug.LogError(cutDateTime);
        }
        
        PlayerAccountManager.instance.SetCreditAccount(data.accountData.data.Balance, cutDateTime.ToString(), credit);
        PlayerAccountManager.instance.AddPlayerGold(data.accountData.Gold);
    }
}
