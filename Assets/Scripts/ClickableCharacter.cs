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
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, Time.deltaTime*characterSpeed);
        if (followingCharacter)
        {
            if (transform.position.x > minX && transform.position.x < maxX)
                Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
            if (transform.position.y > minY && transform.position.y < maxY)
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
    }
}