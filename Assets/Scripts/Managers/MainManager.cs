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
        if (PlayerAccountManager.instance.CreditAccount.InterestRate == 0)
        {
            OpenFirstCreditDialog();
        }
        else
        {
            DateTime cutDate = Convert.ToDateTime(PlayerAccountManager.instance.CreditAccount.CutDate);
            if (DateTime.Now > cutDate)
            {/*
                Debug.LogError("pasamos fecha de corte :v");
                PlayerAccountManager.instance.MakeDailyCutoff();
                RoomManager.GetInstance().avatarData.SetPlayerAccountData(PlayerAccountManager.instance);
                SaveData.Save(RoomManager.GetInstance().avatarData);
                OpenDailyCutoffDialog();*/
            } // end if
        }


    }

    public void AcceptCredit()
    {
        dialog.confirmButton.onClick.RemoveListener(WatchDetails);
        dialog.cancelButton.onClick.RemoveListener(WatchDetails);
        DateTime cutDate = DateTime.Now;
        cutDate.Add(new TimeSpan(0, 5, 0));
        PlayerAccountManager.instance.SetCreditAccount(0, cutDate.ToString(), new Credit(10, 1000));
        PlayerAccountManager.instance.AskEffectiveCredit(1000);
        RoomManager.GetInstance().avatarData.SetPlayerAccountData(PlayerAccountManager.instance);
        SaveData.Save(RoomManager.GetInstance().avatarData);
        dialog.gameObject.SetActive(false);
    }

    public void OpenFirstCreditDialog()
    {
        dialog.gameObject.SetActive(true);
        dialog.warningText.text = "Bienvenido al juego para empezar te ofrecemos un credito inicial de 1000.00MX con una tasa de interes del 10%. Recuerda que el corte o fecha de pago es diario a las 6 am. Trata de mantener tu credito al corriente. Have fun.";
        dialog.confirmButton.onClick.AddListener(AcceptCredit);
        dialog.cancelButton.onClick.AddListener(AcceptCredit);
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
