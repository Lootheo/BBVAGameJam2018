using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvents : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                ShowClickableInfo(hit);
            }
        }
    }

    void ShowClickableInfo(RaycastHit clickableHit)
    {
        ClickableFurniture furniture = clickableHit.transform.GetComponent<ClickableFurniture>();
        ClickableCharacter character = clickableHit.transform.GetComponent<ClickableCharacter>();
        ClickableFloor floor = clickableHit.transform.GetComponent<ClickableFloor>();

        if (furniture)
        {
            Debug.Log(furniture.Name);
            Debug.Log(furniture.description);
            if (furniture.purchased)
            {
                Debug.Log("not purchased");
            }
            else
            {
                Debug.Log("This item is already purchased");
            }
        }
        else if (character)
        {
            Debug.Log(character.name);
            Debug.Log(character.level);

        }else if (floor)
        {
            Debug.Log("hitting floor");
            Debug.Log(clickableHit.point);
        }


    }
}
