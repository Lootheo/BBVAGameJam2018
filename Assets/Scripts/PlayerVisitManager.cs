using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerVisitManager : MonoBehaviour {
    private static PlayerVisitManager instance = null;
    public ServerData hostData;
    public List<Item> allItems;
    public CharacterHolder holder;
    public PlayerVisitSender pvs;

    public static PlayerVisitManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerVisitManager();
        }

        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pvs = GameObject.FindObjectOfType<PlayerVisitSender>();
        if (pvs!=null)
        {
            hostData = pvs.playerToVisit;
            SetHostCharacter();
            ConstructRoom();
        }
    }

    public void ReturnToOwnHome()
    {
        Destroy(pvs.gameObject);
        SceneManager.LoadScene("MainScene");
    }

    public List<Item> ownedClothes = new List<Item>();
    void SetHostCharacter()
    {
        foreach (int itemID in hostData.purchasedItems)
        {
            Item purchasedItem = allItems.Find(x => x.itemID == itemID);
            ownedClothes.Add(purchasedItem);
        }
        ownedClothes = ownedClothes.Where(x => x.itemType == ItemType.Cloth).ToList();
        SetCharacterLook();
    }

    void SetCharacterLook()
    {
        foreach (int itemID in hostData.avatarItems)
        {
            Item purchasedItem = ownedClothes.Find(x => x.itemID == itemID);
            if (purchasedItem != null)
            {
                holder.SetItemPiece(purchasedItem);
            }
        }
    }

    public GameObject furniturePrefab;
    void ConstructRoom()
    {
        foreach (Furniture item in hostData.houseItems)
        {
            Item owned = allItems.Find(x => x.itemID == item.furnitureID && item.displayed == true);
            if (owned != null)
            {
                GameObject itemInstance = Instantiate(furniturePrefab, new Vector3(item.positionX, item.positionY, -5f), Quaternion.identity) as GameObject;
                itemInstance.GetComponent<SpriteRenderer>().sprite = owned.itemGraphic;
                itemInstance.AddComponent<PolygonCollider2D>();
            }
        }
    }
}
