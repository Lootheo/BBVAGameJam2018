using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PromoDialog : MonoBehaviour {

    public TextMeshProUGUI currentInterest;
    public TextMeshProUGUI nextInterest;
    public TextMeshProUGUI currentCreditLimit;
    public TextMeshProUGUI nextCreditLimit;
    public Button CreditButton;
    public Button InterestButton;
    public Button RejectPromoButton;

    public int interestUpgradeRate = 2;
    public int creditLimitUpgradeRate = 100;

    private int _nextCredit;
    private int _nextInterest;

    public void OpenPromoDialog()
    {
        gameObject.SetActive(true);
        int _currentInterest = PlayerAccountManager.instance.CreditAccount.InterestRate;
        _nextInterest = _currentInterest - interestUpgradeRate;
        int _currentCredit = PlayerAccountManager.instance.CreditAccount.CurrentCredit.CreditLimit;
        _nextCredit = _currentCredit + (int)(_currentCredit * (creditLimitUpgradeRate * 0.01f));

        currentInterest.text = _currentInterest.ToString() + " %";
        nextInterest.text = _nextInterest.ToString() + " %"; 

        currentCreditLimit.text = _currentCredit.ToString() + ".00MX";
        nextCreditLimit.text = _nextCredit.ToString() + ".00MX";

    }

    public void CreditButtonAction()
    {
        Credit upgradedCredit = new Credit(PlayerAccountManager.instance.CreditAccount.InterestRate, _nextCredit);
        PlayerAccountManager.instance.SetCreditAccount(PlayerAccountManager.instance.CreditAccount.Balance,
            PlayerAccountManager.instance.CreditAccount.CutDate,
            upgradedCredit);

    }

    public void InterestButtonAction()
    {
        Credit upgradedCredit = new Credit(_nextInterest, PlayerAccountManager.instance.CreditAccount.CurrentCredit.CreditLimit);
        PlayerAccountManager.instance.SetCreditAccount(PlayerAccountManager.instance.CreditAccount.Balance,
            PlayerAccountManager.instance.CreditAccount.CutDate,
            upgradedCredit);
    }

    public void RejectButtonAction()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        RoomManager.GetInstance().avatarData.SetPlayerAccountData(PlayerAccountManager.instance);
        SaveData.Save(RoomManager.GetInstance().avatarData);
    }

}
