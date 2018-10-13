using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainManager : MonoBehaviour {



    private void Start()
    {
        DateTime cutDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
        Credit credit = new Credit(10, 10000);
        if (DateTime.Now.Hour >= 6)
        {
            TimeSpan span = new TimeSpan(24, 0, 0);
            cutDateTime.Add(span);
            Debug.LogError(cutDateTime);
        }
        PlayerAccountManager.instance.SetCreditAccount(0, cutDateTime.ToString(), credit);
        PlayerAccountManager.instance.AddPlayerGold(100);
    }
}
