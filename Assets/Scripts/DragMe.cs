using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool dragOnSurfaces = true;
    public Item itemData;
	public DataSender m_DraggingIcon;
	private RectTransform m_DraggingPlane;
    public int publicValue;
    public Image icon;
    Canvas canvas;

    public void SetData(DataSender sender, Canvas parent, Item itemData)
    {
        this.itemData = itemData;
        m_DraggingIcon = sender;
        canvas = parent;
    }

	public void OnBeginDrag(PointerEventData eventData)
	{
        if (m_DraggingIcon == null)
            return;

        m_DraggingIcon.gameObject.SetActive(true);
        // We have clicked something that can be dragged.
        // What we want to do is create an icon for this.
        m_DraggingIcon.dataToSend = publicValue;
		
		var image = m_DraggingIcon.iconHolder;
        image.color = icon.color;
		// The icon will be under the cursor.
		// We want it to be ignored by the event system.

		image.sprite = icon.sprite;
		//image.SetNativeSize();
        image.rectTransform.sizeDelta = new Vector2(100, 100);
		
		m_DraggingPlane = canvas.transform as RectTransform;
			
		
		SetDraggedPosition(eventData);
	}

	public void OnDrag(PointerEventData data)
	{
		if (m_DraggingIcon != null)
			SetDraggedPosition(data);
	}

	private void SetDraggedPosition(PointerEventData data)
	{
		if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
			m_DraggingPlane = data.pointerEnter.transform as RectTransform;
		
		var rt = m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
			rt.rotation = m_DraggingPlane.rotation;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
        if (m_DraggingIcon != null)
            m_DraggingIcon.gameObject.SetActive(false);
	}
}
