using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragBuild : DragClass
{
    public Object GoldGeneratorPrefab;
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
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        Transform mainCanvas = GameObject.Find("Canvas").transform;
        Vector3 goldChargePrefabPos = Camera.main.ScreenToWorldPoint(data.position);
        if (itemData.furnitureType != FurnitureType.None && itemData.furnitureType != FurnitureType.Shelf)
        {
            Instantiate(GoldGeneratorPrefab, new Vector3(goldChargePrefabPos.x, goldChargePrefabPos.y, -10), Quaternion.identity, mainCanvas);
        }else if(itemData.furnitureType == FurnitureType.Shelf)
        {
        }
        if (hit.collider!=null)
        {
            switch (itemData.furnitureType)
            {
                case FurnitureType.Bed:
                case FurnitureType.Bookshelf:
                case FurnitureType.Chair:
                case FurnitureType.Fireplace:
                case FurnitureType.Shelf:
                case FurnitureType.Table:
                    if (hit.collider.tag == "Floor")
                    {
                        ConstructionManager.instance.BuildItemAtPosition(itemData, new Vector3(worldPos.x, worldPos.y, -5));
                    }
                    break;
                case FurnitureType.Window:
                case FurnitureType.Door:
                    if (hit.collider.tag == "Wall")
                    {
                        ConstructionManager.instance.BuildItemAtPosition(itemData, new Vector3(worldPos.x, worldPos.y, -5));
                    }
                    break;
                default:
                    break;
            }
            if (itemData.itemType == ItemType.Room)
            {
                ConstructionManager.instance.ChangeRoom(itemData);
            }
        }


        FindObjectOfType<ClickableCharacter>().GetComponent<BoxCollider2D>().enabled = true;
        FindObjectOfType<ClickableCharacter>().followingCharacter = true;
        FindObjectOfType<ClickableCharacter>().canMove = true;
    }
}
