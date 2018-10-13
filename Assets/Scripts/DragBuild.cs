using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragBuild : DragClass
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcon != null)
            m_DraggingIcon.gameObject.SetActive(false);
        BuildOnDragPoint(eventData);
    }

    void BuildOnDragPoint(PointerEventData data)
    {
        Debug.Log(data.position);
    }
}
