using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HistorialDeCompra : MonoBehaviour
{
    public TextMeshProUGUI[] mov1;

    public void OpenWindow()
    {
        this.gameObject.SetActive(true);
        for(int i = 0; i < 4; i++)
        {
            int count = PlayerAccountManager.instance.transactions.Count - 1;
            if(count > 0)
            {
                mov1[i].text = "Detalle: " + PlayerAccountManager.instance.transactions[count].Description + ". Pago: " +
                    PlayerAccountManager.instance.transactions[count].Amount + ". Fecha: " + PlayerAccountManager.instance.transactions[count].TransactionDate;
            }
            else
            {
                mov1[i].text = "No hay suficientes movimientos";
            }
            
        }
         
    }
}
