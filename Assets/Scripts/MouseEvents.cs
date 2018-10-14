using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MouseEvents : MonoBehaviour {

    public TextMeshProUGUI nameText, descriptionText, priceText;
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                ShowClickableInfo(hit);
            }
        }
    }

    void ShowClickableInfo(RaycastHit2D clickableHit)
    {
        ClickableItem clicked = clickableHit.transform.GetComponent<ClickableItem>();
        if (clicked)
        {
            clicked.OnItemClick();
        }
        /*if (furniture)
        {
            Debug.Log(furniture.Name);
            Debug.Log(furniture.description);
            nameText.text = furniture.Name;
            descriptionText.text = furniture.description;
            priceText.text = furniture.price.ToString();
            if (furniture.purchased)
            {
                Debug.Log("purchased");
            }
            else
            {
                Debug.Log("not purchased");
            }
        }
        else if (character)
        {
            Debug.Log(character.name);
            Debug.Log(character.level);

        }else if (floor)
        {
            Debug.Log("hitting floor");
            FindObjectOfType<ClickableCharacter>().MoveCharacter(clickableHit.point);
        }else if (door)
        {
            door.Activate();
        }*/
    }
}
