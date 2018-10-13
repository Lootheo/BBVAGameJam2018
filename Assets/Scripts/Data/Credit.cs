using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreditAccountState
{
    private int balance;
    private string cutDate;

    private int minPayment;
    private int availableCredit;

    private Credit currentCredit;
    private List<Transaction> transactions;

    public Credit CurrentCredit
    {
        get { return currentCredit; }
    }

    public int InterestRate
    {
        get { return currentCredit.InterestRate; }
    }

    public int Balance
    {
        set
        {
            balance = value;
            minPayment = Mathf.CeilToInt(balance * 0.1f);
            availableCredit = currentCredit.CreditLimit - balance;
        }

        get { return balance; }
    }

    public int MinPayment
    {
        get { return minPayment; }
    }

    public int AvailableCredit
    {
        get { return availableCredit; }
    }

    public CreditAccountState(int _balance, string _cutDate, Credit _currentCredit, List<Transaction> _transactions)
    {
        currentCredit = _currentCredit;
        transactions = _transactions;
        Balance = _balance;
        cutDate = _cutDate;
    }

    public CreditAccountState(int _balance, string _cutDate, Credit _currentCredit)
    {
        currentCredit = _currentCredit;
        transactions = new List<Transaction>();
        Balance = _balance;
        cutDate = _cutDate;
    }

    public void UpdateCredit(Credit _currentCredit)
    {
        currentCredit = _currentCredit;
        availableCredit = currentCredit.CreditLimit - balance;
    }

    public void AddTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
        Balance += transaction.Amount;
    }
    /*
    public void AddToBalance(int amount)
    {
        Balance -= amount;
        AddTransaction(new Transaction("Payed amount " + amount, -amount, DateTime.))
    }*/
}

[System.Serializable]
public class Credit
{
    private int interestRate;
    private int creditLimit;

    public int InterestRate
    {
        get { return interestRate; }
    }

    public int CreditLimit
    {
        get { return creditLimit; }
    }

    public Credit(int _interestRate, int _creditLimit)
    {
        interestRate = _interestRate;
        creditLimit = _creditLimit;
    }
}

[System.Serializable]
public class Transaction
{
    private string description;
    private int amount;
    private string date;
    
    public string Description
    {
        get { return description; }

    }

    public int Amount
    {
        get { return amount; }
    }

    public string TransactionDate
    {
        get { return date; }
    }

    public Transaction(string _description, int _amount, string _date)
    {
        description = _description;
        amount = _amount;
        date = _date;
    }

}
