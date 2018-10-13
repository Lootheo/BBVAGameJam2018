using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class PlayerAccountManager : MonoBehaviour
{
    public static PlayerAccountManager instance;
    public int Gold;
    public CreditAccountState CreditAccount;
    public List<Transaction> transactions;
    public List<CreditAccountState> WeeklyCreditState;
    public TextMeshProUGUI PlayerPointsText;

    private void Awake()
    {
        instance = this;
        transactions = new List<Transaction>();
        WeeklyCreditState = new List<CreditAccountState>();
        
    }

    public void AddPlayerGold(int points)
    {
        Gold += points;
    }

    public void BuyWithGold(Item item)
    {
        Gold -= item.itemPrice;
        AddTransaction(new Transaction(item.name + " payed with points", item.itemPrice, DateTime.Now.ToString()));
    }

    public void AddTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
    }

    public void BuyWithCredit(Item item)
    {
        Transaction transaction = new Transaction(item.name + " payed with credit", item.itemPrice, DateTime.Now.ToString());
        AddTransaction(transaction);
        CreditAccount.AddTransaction(transaction);
    }

    public void PayCredit(int playerPoints)
    {
        Transaction transaction = new Transaction("Payed amount to credit " + playerPoints, -playerPoints, DateTime.Now.ToString());
        CreditAccount.AddTransaction(transaction);
        Gold -= playerPoints;
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

        int newBalance = CreditAccount.Balance + (int)(CreditAccount.Balance * (CreditAccount.InterestRate * 0.01f));
        SetCreditAccount(newBalance, "en 24hrs", CreditAccount.CurrentCredit);
    }

    public void SetCreditAccount(int balance, string cutDate, Credit credit)
    {
        CreditAccount = new CreditAccountState(balance, cutDate, credit);
    }

    public void Update()
    {
        PlayerPointsText.text = Gold.ToString();
    }
}
