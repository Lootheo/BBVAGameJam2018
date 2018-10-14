using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VisitManager : MonoBehaviour {
    public PlayerVisitSender dataCarrier;
    public RectTransform hostListRect;
    public GameObject hostPrefab;
    public FinalFirebaseConnection ffbc;
    public List<GameObject> hostEntries = new List<GameObject>();
	
    public void FillEntryList()
    {
        ffbc.ShowFriends();
        ClearShopObjects();
        foreach (ServerData item in ffbc.playerData)
        {
            GameObject itemInstance = Instantiate(hostPrefab, hostListRect);
            VisitEntry entry = itemInstance.GetComponent<VisitEntry>();
            entry.SetData(item);
            entry.visitBtn.onClick.AddListener(() => GoToHostHouse(item));
            hostListRect.sizeDelta += new Vector2(0, 80);
            hostEntries.Add(itemInstance);
        }
    }

    public void GoToHostHouse(ServerData hostData)
    {
        dataCarrier.playerToVisit = hostData;
        SceneManager.LoadScene("VisitPlayer");
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("MatchThree");
    }

    void ClearShopObjects()
    {
        foreach (GameObject item in hostEntries)
        {
            Destroy(item);
        }
        hostEntries.Clear();
        hostListRect.sizeDelta = new Vector2(hostListRect.sizeDelta.x, 0);
    }
}
