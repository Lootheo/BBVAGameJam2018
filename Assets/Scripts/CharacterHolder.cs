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
        avatarSprites[(int)piece.clothType].transform.localPosition = piece.itemOffset;
        avatarSprites[(int)piece.clothType].sprite = piece.itemGraphic;
        Item tempPiece = currentItems.Find(x => x.clothType == piece.clothType);
        if (tempPiece!=null)
        {
            currentItems.Remove(tempPiece);
        }
        currentItems.Add(piece);
    }
}
