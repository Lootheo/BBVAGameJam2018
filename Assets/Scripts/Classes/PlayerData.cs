using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    public List<int> purchasedItems;
    public List<Furniture> houseItems;
    public List<int> avatarItems;
    public string avatarName;
    public PlayerAccountData accountData;

    public PlayerData()
    {
        avatarItems = new List<int>();
        houseItems = new List<Furniture>();
        purchasedItems = new List<int>();
        avatarName = "";
    }

    public PlayerData(List<int> items, string name, PlayerAccountData _accountData)
    {
        avatarItems = items;
        houseItems = new List<Furniture>();
        purchasedItems = items;
        avatarName = name;
        accountData = _accountData;
    }

    public PlayerData(List<int> items, string name)
    {
        avatarItems = items;
        houseItems = new List<Furniture>();
        purchasedItems = items;
        avatarName = name;
    }

    public void SetPlayerAccountData(PlayerAccountManager pm)
    {
        CreditAccountData data = new CreditAccountData(pm.CreditAccount.Balance, pm.CreditAccount.CutDate, pm.CreditAccount.InterestRate, pm.CreditAccount.CurrentCredit.CreditLimit, pm.CreditAccount.transactions);
        List<CreditAccountData> historial = new List<CreditAccountData>();
        for (int i = 0; i < pm.WeeklyCreditState.Count; i++)
        {
            historial.Add(new CreditAccountData(pm.WeeklyCreditState[i].Balance, pm.WeeklyCreditState[i].CutDate, pm.WeeklyCreditState[i].InterestRate, pm.WeeklyCreditState[i].CurrentCredit.CreditLimit, pm.WeeklyCreditState[i].transactions));
        }
        accountData = new PlayerAccountData(pm.Gold, pm.CreditAccount.InterestRate, pm.CreditAccount.CurrentCredit.CreditLimit, data, historial, pm.transactions);
    }
}

[System.Serializable]
public class Furniture
{
    public int furnitureID;
    public float positionX, positionY;
    public bool displayed;

    public Furniture(int id, float pos_x, float pos_y, bool disp)
    {
        furnitureID = id;
        positionX = pos_x;
        positionY = pos_y;
        displayed = disp;
    }
}

[System.Serializable]
public class ServerData
{
    public List<int> purchasedItems;
    public List<Furniture> houseItems;
    public List<int> avatarItems;
    public string avatarName;

    public ServerData()
    {
        avatarItems = new List<int>();
        houseItems = new List<Furniture>();
        purchasedItems = new List<int>();
        avatarName = "";
    }

    public ServerData(PlayerData data)
    {
        avatarItems = data.avatarItems;
        houseItems = data.houseItems;
        purchasedItems = data.purchasedItems;
        avatarName = data.avatarName;
    }
}

[System.Serializable]
public class PlayerAccountData
{
    public int Gold;
    public int InterestRate;
    public int CreditLimit;
    public CreditAccountData currentCreditAccountData;
    public List<CreditAccountData> weeklyHistorial;
    public List<Transaction> allTransactions;

    public PlayerAccountData(int _gold, int _interestRate, int _credit, CreditAccountData _data, List<CreditAccountData> _historial, List<Transaction> _transactions)
    {
        Gold = _gold;
        InterestRate = _interestRate;
        CreditLimit = _credit;
        currentCreditAccountData = _data;
        weeklyHistorial = _historial;
        allTransactions = _transactions;
    }
}

[System.Serializable]
public class CreditAccountData
{
    public int Balance;
    public string CutDate;
    public List<Transaction> transactions;
    public int interestRate;
    public int creditLimit;

    public CreditAccountData()
    {
        Balance = 0;
        DateTime cutDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
        if (DateTime.Now.Hour > 6)
        {
            TimeSpan timespan = new TimeSpan(24, 0, 0);
            cutDate.Add(timespan);
        } // end if
        CutDate = cutDate.ToString();
        transactions = new List<Transaction>();
    }

    public CreditAccountData(int _balance, string _cutDate, int _interest, int _creditLimit, List<Transaction> _transactions)
    {
        Balance = _balance;
        CutDate = _cutDate;
        transactions = _transactions;
        interestRate = _interest;
        creditLimit = _creditLimit;
    }
}
