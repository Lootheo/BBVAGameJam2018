using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldGenerator : MonoBehaviour {
    public int secondsToGenerate=5;
    public int amountGenerated;
    private float currentTime=0;
    public Image assignedChargeBar;
    public float assignedBarOffset;
    public Object particles;

    public void Update()
    {
        if (GetComponent<ClickableFurniture>().purchased)
        {
            assignedChargeBar.transform.parent.gameObject.SetActive(true);
            assignedChargeBar.transform.parent.position = transform.position +Vector3.up*assignedBarOffset;
            currentTime += Time.deltaTime;

            if (currentTime > secondsToGenerate)
            {
                currentTime = 0;
                FindObjectOfType<PlayerAccountManager>().Gold += amountGenerated;
                Vector3 positionToInstantiate = new Vector3(transform.position.x, transform.position.y, -5f);
                GameObject bills= Instantiate(particles, positionToInstantiate, Quaternion.identity) as GameObject;
                bills.GetComponent<ParticleSystem>().Play();
            }
            assignedChargeBar.fillAmount = (float)currentTime / secondsToGenerate;
        }
        else
        {
            assignedChargeBar.transform.parent.gameObject.SetActive(false);
        }
       
    }
}
