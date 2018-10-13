using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData{
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
        purchasedItems = new List<int>();
        avatarName = name;
        accountData = _accountData;
    }
}

[System.Serializable]
public class Furniture
{
    public int furnitureID;
    public float positionX, positionY;
    public bool displayed;

    public Furniture(int id, Vector2 position, bool disp)
    {
        furnitureID = id;
        positionX = position.x;
        positionY = position.y;
        displayed = disp;
    }
}

[System.Serializable]
public class PlayerAccountData
{
    public int Gold;
    public int InterestRate;
    public int CreditLimit;
    public CreditAccountData data;

    public PlayerAccountData(int _gold, int _interestRate, int _credit, CreditAccountData _data)
    {
        Gold = _gold;
        InterestRate = _interestRate;
        CreditLimit = _credit;
        data = _data;
    }
}

[System.Serializable]
public class CreditAccountData
{
    public int Balance;
    public string CutDate;
    public List<Transaction> transactions;
    
    public CreditAccountData()
    {
        transactions = new List<Transaction>();
    }

    public CreditAccountData(int _balance, string _cutDate, List<Transaction> _transactions)
    {
        Balance = _balance;
        CutDate = _cutDate;
        transactions = _transactions;
    }
}


