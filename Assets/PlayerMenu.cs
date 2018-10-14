using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour {
    private void OnDisable()
    {
        FindObjectOfType<ClickableCharacter>().GetComponent<BoxCollider2D>().enabled = true;
        FindObjectOfType<ClickableCharacter>().followingCharacter = true;
        FindObjectOfType<ClickableCharacter>().canMove = true;
    }
}
