using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainManager : MonoBehaviour {

    public UI_Dialog dialog;
    public UI_PromoDialog promoDialog;

    private void Start()
    {
        PlayerAccountManager.instance.SetAccountData();
        if(PlayerAccountManager.instance.CreditAccount.InterestRate == 0)
        {
            Debug.LogError("ofrecer credito");
        }
        /*
        DateTime cutDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
        PlayerData data = SaveData.Load();
        Credit credit;
        bool alreadyHaveCredit = false;
        if (data.accountData.InterestRate != 0) // ya tiene un credito
        {
            alreadyHaveCredit = true;
            credit = new Credit(data.accountData.InterestRate, data.accountData.CreditLimit);
            cutDateTime = DateTime.Parse(data.accountData.currentCreditAccountData.CutDate);
        } // end if 
        else // le creamos su primer credito
        {
            credit = new Credit(10, 1000);
            if (DateTime.Now.Hour >= 6 && alreadyHaveCredit) //checar si ya paso la fecha de corte
            {
                TimeSpan span = new TimeSpan(24, 0, 0);
                cutDateTime.Add(span);
            }// end if
        }

        PlayerAccountManager.instance.SetCreditAccount(data.accountData.currentCreditAccountData.Balance, cutDateTime.ToString(), credit);
        PlayerAccountManager.instance.AddPlayerGold(data.accountData.Gold);

        if (DateTime.Now.Hour >= 6 && alreadyHaveCredit) //checar si ya paso la fecha de corte
        {
            PlayerAccountManager.instance.MakeDailyCutoff();
            //avisar que se realizo el corte diario
            if(PlayerAccountManager.instance.CreditAccount.Balance == 0)
            {
                //ofrecer mejora de credito en un dialog
            }
        }
        */

        
    }

    public void OpenDailyCutoffDialog()
    {
        dialog.transform.parent.gameObject.SetActive(true);
        dialog.warningText.text = "Tu saldo al corte es de: " + PlayerAccountManager.instance.WeeklyCreditState[
            PlayerAccountManager.instance.WeeklyCreditState.Count - 1].Balance + ".00MX. Tu nuevo saldo es de "
            + PlayerAccountManager.instance.CreditAccount.Balance + ".00MX.";
        dialog.confirmButton.onClick.AddListener(WatchDetails);
        //dialog.confirmButton.onClick.AddListener(CloseWindow);
        dialog.cancelButton.onClick.AddListener(VerifyForAugment);
    }

    public void CloseWindow()
    {
        dialog.transform.parent.gameObject.SetActive(false);
    }

    public void VerifyForAugment()
    {
        dialog.cancelButton.onClick.RemoveListener(VerifyForAugment);
        
        dialog.confirmButton.onClick.RemoveListener(WatchDetails);

        if(PlayerAccountManager.instance.CreditAccount.Balance == 0)
        {
            OpenPromoDialog();
        } // end if
    }

    public void OpenPromoDialog()
    {
        dialog.transform.parent.gameObject.SetActive(true);
        dialog.warningText.text = "Felicidades! Haz completado el dia sin una deuda, por eso Bancomer te ofrece mejorar una caracteristica de tu credito si asi lo deseas.";
        dialog.confirmButton.onClick.AddListener(OpenUpgradeCreditWindow);
    }

    public void OpenUpgradeCreditWindow()
    {
        dialog.confirmButton.onClick.RemoveListener(OpenUpgradeCreditWindow);
        promoDialog.OpenPromoDialog();
    }


    public void WatchDetails()
    {
        // open another modal to watch the details of the account state
    }

    
}
