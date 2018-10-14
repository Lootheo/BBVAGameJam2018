using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : MonoBehaviour {
    public RectTransform gridRect;
    public GameObject itemPrefab;
    public CharacterHolder holder;
    public GameObject mainScreen, characterScreen;
    public List<Item> starterItems;
    public DataSender sender;
    public Canvas mainCanvas;
    public InputField nameField;

    List<DragMe> entries = new List<DragMe>();

    private static MainMenu instance = null;


    public static MainMenu GetInstance()
    {
        if (instance == null)
        {
            instance = new MainMenu();
        }

        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        FillGrid(1);
	}

    public void NewPlayer()
    {
        mainScreen.SetActive(false);
        characterScreen.SetActive(true);
        holder.gameObject.SetActive(true);
    }

    public void FillGrid(int itemType)
    {
        List<Item> filteredList = new List<Item>();
        filteredList = starterItems.Where(x => x.clothType == (ClothType)itemType).ToList();
        FillShopItems(filteredList);
    }

    void FillShopItems(List<Item> shopItems)
    {
        ClearGrid();
        foreach (Item item in shopItems)
        {
            GameObject itemInstance = Instantiate(itemPrefab, gridRect);
            DragMe shopItem = itemInstance.GetComponent<DragMe>();
            shopItem.SetData(sender, mainCanvas, item);
            shopItem.icon.sprite = item.itemGraphic;
            entries.Add(shopItem);
        }
    }

    void ClearGrid()
    {
        foreach (DragMe item in entries)
        {
            Destroy(item.gameObject);
        }
        entries.Clear();
    }
}
