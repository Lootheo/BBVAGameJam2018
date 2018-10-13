using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Dialog : MonoBehaviour
{

    public TextMeshProUGUI text;
    
    public void OpenBuyConfirmationWindow(Item item)
    {
        gameObject.transform.parent.gameObject.SetActive(true);
        text.text = "El objeto " + item.name + " tiene un costo de " + item.itemPrice + ".00MX y tienes un credito disponible de"
            + PlayerAccountManager.instance.CreditAccount.AvailableCredit + ".00MX. Presiona Aceptar para confirmar la compra.";
        GetComponent<Button>().onClick.AddListener(()=>StoreManager.instance.BuyWithCredit(item));
    }

    public void OpenDeniedShopingWindow(Item item)
    {
        gameObject.transform.parent.gameObject.SetActive(true);
        text.text = "No tienes suficiente credito para comprar el objeto " + item.name + ". Credito disponible: " +
            PlayerAccountManager.instance.CreditAccount.AvailableCredit + ".00MX. Te sugerimos saldar tu cuenta para comprar este objeto.";
        GetComponent<Button>().onClick.AddListener(CloseWindow);
    }

    public void CloseWindow()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }

}
