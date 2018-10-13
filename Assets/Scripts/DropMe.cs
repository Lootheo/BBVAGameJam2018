using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DropMe : MonoBehaviour, IDropHandler
{
    public CharacterHolder holder;
    public int secretValue;
    //public Image checkImage;
    public Sprite[] sprites;

    void Start()
    {
        //checkImage.gameObject.SetActive(false);
    }
	
	public void OnDrop(PointerEventData data)
	{
		GetDropSprite (data);
	}

	private void GetDropSprite(PointerEventData data)
	{
		var originalObj = data.pointerDrag;
        Debug.Log(originalObj.name);
        DragMe dm = originalObj.GetComponent<DragMe>();
        holder.SetItemPiece(dm.itemData);
	}
}
