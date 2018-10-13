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
        //transform.position = new Vector3(positionToMove.x, positionToMove.y, transform.position.z);
    }
    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, Time.deltaTime*characterSpeed);
        if (followingCharacter)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
    }
}