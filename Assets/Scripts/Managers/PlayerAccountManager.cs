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
    public TextMeshProUGUI playerCreditText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        CreditAccount = new CreditAccountState(0, DateTime.Now.ToString(), new Credit(0, 0));
        transactions = new List<Transaction>();
        WeeklyCreditState = new List<CreditAccountState>();
        
    }

    public void SetAccountData()
    {
        PlayerAccountData accountData = RoomManager.GetInstance().avatarData.accountData;
        CreditAccountData creditData = accountData.currentCreditAccountData;
        List<Transaction> allTransactions = accountData.allTransactions;
        List<CreditAccountData> historial = new List<CreditAccountData>();
        for(int i = 0; i < accountData.weeklyHistorial.Count; i++)
        {
            historial.Add(accountData.weeklyHistorial[i]);
        }

        CreditAccount = new CreditAccountState(creditData.Balance, creditData.CutDate, new Credit(accountData.InterestRate, accountData.CreditLimit));
        transactions = allTransactions;
        for(int i = 0; i < historial.Count; i++)
        {
            WeeklyCreditState.Add(new CreditAccountState(historial[i].Balance, historial[i].CutDate,
                new Credit(historial[i].interestRate, historial[i].creditLimit)));
        } // end for

    }

    private void Update()
    {
        if (PlayerPointsText)
            PlayerPointsText.text = Gold.ToString() + ".00MX";
        if (playerCreditText)
            playerCreditText.text = CreditAccount.Balance.ToString() + ".00MX";
    }

    public void AskEffectiveCredit(int amount)
    {
        Transaction transaction = new Transaction( "Prestamo", amount, DateTime.Now.ToString());
        AddTransaction(transaction);
        CreditAccount.AddTransaction(transaction);
        Gold += amount;
    }

    public void AddPlayerGold(int points)
    {
        Gold += points;
    }

    public void BuyWithGold(Item item)
    {
        Gold -= item.itemPrice;
        AddTransaction(new Transaction(item.name + " pagado en efectivo ", item.itemPrice, DateTime.Now.ToString()));
    }

    public void AddTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
    }

    public void BuyWithCredit(Item item)
    {
        Transaction transaction = new Transaction(item.name + " pagado con credito ", item.itemPrice, DateTime.Now.ToString());
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
        DateTime cutDate = Convert.ToDateTime(CreditAccount.CutDate);
        TimeSpan timespan = new TimeSpan(24, 0, 0);
        cutDate.Add(timespan);
        SetCreditAccount(newBalance, cutDate.ToString(), CreditAccount.CurrentCredit);
    }

    public void SetCreditAccount(int balance, string cutDate, Credit credit)
    {
        CreditAccount = new CreditAccountState(balance, cutDate, credit);
    }
}
