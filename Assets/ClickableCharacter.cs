using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableCharacter:MonoBehaviour
{
    //Character data
    public string Name;
    public string description;
    public int level;
    public Sprite image;
    public Vector2 finalPosition;
    public float characterSpeed = 1.0f;
    public bool followingCharacter = false;
    public void MoveCharacter(Vector2 positionToMove)
    {
        finalPosition = positionToMove;
        //transform.position = new Vector3(positionToMove.x, positionToMove.y, transform.position.z);
    }
    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, Time.deltaTime*characterSpeed);
        if (followingCharacter)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
    }
}