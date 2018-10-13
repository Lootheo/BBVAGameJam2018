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
    public List<CreditAccountState> WeeklyCreditState;

    private void Awake()
    {
        instance = this;
        transactions = new List<Transaction>();
        WeeklyCreditState = new List<CreditAccountState>();
    }

    public void AddPlayerPoints(int points)
    {
        PlayerPoints += points;
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

    public void PayCredit(int playerPoints)
    {
        Transaction transaction = new Transaction("Payed amount to credit " + playerPoints, -playerPoints, DateTime.Now.ToString());
        CreditAccount.AddTransaction(transaction);
        PlayerPoints -= playerPoints;
    }

    public void MakeDailyCutoff()
    {
        if(WeeklyCreditState.Count < 7)
        {
            WeeklyCreditState.Add(CreditAccount);
        } // end if
        else
        {
            for(int i = 0; i < WeeklyCreditState.Count - 1; i++)
            {
                WeeklyCreditState[i] = WeeklyCreditState[i + 1];
            } // end for
            WeeklyCreditState[WeeklyCreditState.Count - 1] = CreditAccount;
        } // end else

        int newBalance = CreditAccount.Balance + (CreditAccount.Balance * CreditAccount.InterestRate);
        SetCreditAccount(newBalance, "en 24hrs", CreditAccount.CurrentCredit);
    }

    public void SetCreditAccount(int balance, string cutDate, Credit credit)
    {
        CreditAccount = new CreditAccountState(balance, cutDate, credit);
    }
}
