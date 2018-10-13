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
        PlayerPoints -= item.price;
        AddTransaction(new Transaction(item.name + " payed with points", item.price, DateTime.Now.ToString()));
    }

    public void AddTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
    }

    public void BuyWithCredit(Item item)
    {
        PlayerPoints = item.price;
        Transaction transaction = new Transaction(item.name + " payed with credit", item.price, DateTime.Now.ToString());
        AddTransaction(transaction);
        CreditAccount.AddTransaction(transaction);
    }
}
