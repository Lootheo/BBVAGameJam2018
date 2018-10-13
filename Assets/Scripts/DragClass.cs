using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class DragClass : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item itemData;
    public DataSender m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    public Image icon;
    Canvas canvas;

    public void SetData(DataSender sender, Canvas parent, Item itemData)
    {
        this.itemData = itemData;
        m_DraggingIcon = sender;
        canvas = parent;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (m_DraggingIcon == null)
            return;

        m_DraggingIcon.gameObject.SetActive(true);

        var image = m_DraggingIcon.iconHolder;
        image.color = icon.color;
        // The icon will be under the cursor.
        // We want it to be ignored by the event system.

        image.sprite = icon.sprite;
        //image.SetNativeSize();
        image.rectTransform.sizeDelta = icon.rectTransform.sizeDelta;

        m_DraggingPlane = canvas.transform as RectTransform;


        SetDraggedPosition(eventData);
    }

    public virtual void OnDrag(PointerEventData data)
    {
        if (m_DraggingIcon != null)
            SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcon != null)
            m_DraggingIcon.gameObject.SetActive(false);
    }
}
