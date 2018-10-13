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
        Camera main = Camera.main;
        Vector2 worldPos = main.ScreenToWorldPoint(new Vector2(data.position.x, data.position.y));
        Debug.Log(worldPos);
        ConstructionManager.instance.BuildItemAtPosition(itemData, new Vector3(worldPos.x, worldPos.y, -5));
    }
}
