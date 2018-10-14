using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_PayCreditDialog : MonoBehaviour
{
    public TMP_InputField input;
    public TextMeshProUGUI debt;
    public Button savebutton;

    private int maxAmount;

    public void ShowPaymentDialog()
    {
        this.gameObject.SetActive(true);
        if (PlayerAccountManager.instance.Gold >= PlayerAccountManager.instance.CreditAccount.Balance)
            maxAmount = PlayerAccountManager.instance.CreditAccount.Balance;
        else maxAmount = PlayerAccountManager.instance.Gold;
        input.text = "0";
    }

    public void PayAction()
    {
        PlayerAccountManager.instance.PayCredit(Convert.ToInt32(input.text));
        gameObject.SetActive(false);
    }

    private void Update()
    {
        int value = Convert.ToInt32(input.text);
        if(value > maxAmount)
        {
            value = maxAmount;
            input.text = value.ToString();
        }
        int total = PlayerAccountManager.instance.CreditAccount.Balance - value;
        debt.text = total.ToString();
    }


}
