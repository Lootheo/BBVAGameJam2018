using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableCharacter: ClickableItem
{
    //Character data
    public string Name;
    public string description;
    public int level;
    public Sprite image;
    public float minX, maxX;
    public float minY, maxY;
    [HideInInspector]
    public Vector3 finalPosition;
    public float characterSpeed = 1.0f;
    public bool followingCharacter = false;
    public void Awake()
    {
        finalPosition = transform.position;
    }

    bool menuToggle = false;

    public override void OnItemClick()
    {
        Debug.Log("Show concept menu");
        menuToggle = !menuToggle;
        RoomManager.GetInstance().ShowConceptualMenu(transform.position, menuToggle);
    }

    public void MoveCharacter(Vector3 positionToMove)
    {
        finalPosition = new Vector3(positionToMove.x, positionToMove.y, transform.position.z);
        if (finalPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        //transform.position = new Vector3(positionToMove.x, positionToMove.y, transform.position.z);
    }

    public void Update()
    {
       
    }
}