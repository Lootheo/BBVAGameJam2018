using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class CharacterHolder : MonoBehaviour
{
    public List<Item> currentItems;
    public List<SpriteRenderer> avatarSprites;

	// Use this for initialization
	void Start () {
        currentItems = new List<Item>();
	}
	

    public void SetItemPiece(Item piece)
    {
        avatarSprites[(int)piece.itemType].transform.localPosition = piece.itemOffset;
        avatarSprites[(int)piece.itemType].sprite = piece.itemGraphic;
        Item tempPiece = currentItems.Find(x => x.itemType == piece.itemType);
        if (tempPiece!=null)
        {
            currentItems.Remove(tempPiece);
        }
        currentItems.Add(piece);
    }
}
