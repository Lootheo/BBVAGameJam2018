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


    public void MoveCharacter(Vector2 positionToMove)
    {
        transform.position = new Vector3(positionToMove.x, positionToMove.y, transform.position.z);
    }

}