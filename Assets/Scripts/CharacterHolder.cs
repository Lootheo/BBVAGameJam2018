using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour {
    public List<Item> legItems;
    public List<Item> accesoryItems;
    public List<Item> hairstyleItems;
    public List<Item> chestItems;
    public List<Item> feetItems;
    
    public List<Item> currentItems;
    public List<SpriteRenderer> avatarSprites;
    public int[] pieceIndexes = new int[5];
    public int topIndex;
	// Use this for initialization
	void Start () {
        currentItems = new List<Item>();
	}
	

    public void NextItemPiece(int piece)
    {
        Item nextPiece = CheckIndex(piece);
        avatarSprites[piece].transform.position = nextPiece.itemOffset;
        avatarSprites[piece].sprite = nextPiece.itemGraphic;
    }

    Item CheckIndex(int piece)
    {
        int currentIndex = pieceIndexes[(int)piece];
        List<Item> pieceList = new List<Item>();
        switch ((ItemType)piece)
        {
            case ItemType.HairStyle:
                pieceList = hairstyleItems;
                break;
            case ItemType.Accesory:
                pieceList = accesoryItems;
                break;
            case ItemType.Chest:
                pieceList = chestItems;
                break;
            case ItemType.Legs:
                pieceList = legItems;
                break;
            case ItemType.Feet:
                pieceList = feetItems;
                break;
            default:
                break;
        }
        topIndex = pieceList.Count - 1;
        if (currentIndex < topIndex)
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
        pieceIndexes[(int)piece] = currentIndex;
        return pieceList[currentIndex];
    }
}
