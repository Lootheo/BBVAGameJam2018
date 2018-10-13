using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAccountManager : MonoBehaviour
{
    public static PlayerAccountManager instance;
    public int PlayerPoints;
    public CreditAccountState CreditAccount;
    public List<Transaction> transactions;

    private void Awake()
    {
        instance = this;
        transactions = new List<Transaction>();
    }

    public void BuyWithPoints(Item item)
    {
        PlayerPoints -= item.itemPrice;
        AddTransaction(new Transaction(item.name + " payed with points", item.itemPrice, DateTime.Now.ToString()));
    }

    public void AddTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
    }

    public void BuyWithCredit(Item item)
    {
        PlayerPoints = item.itemPrice;
        Transaction transaction = new Transaction(item.name + " payed with credit", item.itemPrice, DateTime.Now.ToString());
        AddTransaction(transaction);
        CreditAccount.AddTransaction(transaction);
    }
}
