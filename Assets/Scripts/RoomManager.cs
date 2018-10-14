using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
    private static RoomManager instance = null;
    public GameObject conceptualUI;
    public PlayerData avatarData;
    public List<Item> allItems;
    public FinalFirebaseConnection ffbc;
    public static RoomManager GetInstance()
    {
        if (instance == null)
        {
            instance = new RoomManager();
        }

        return instance;
    }

    private void Awake()
    {
        instance = this;
        avatarData = SaveData.Load();
    }

    private void Start()
    {
        conceptualUI.SetActive(false);
        ffbc.WriteNewUserData(avatarData.avatarName, new ServerData(avatarData));
    }
    private void Update()
    {
        
    }
    public void ShowConceptualMenu(Vector2 playerPos, bool show)
    {
        if (show)
        {
            Camera.main.orthographicSize = 1.75f;
            Vector3 newCameraPosition = FindObjectOfType<ClickableCharacter>().transform.position;
            FindObjectOfType<ClickableCharacter>().followingCharacter = false;
            FindObjectOfType<ClickableCharacter>().canMove = false;
            Camera.main.transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, Camera.main.transform.position.z);
        }
        else
        {
            FindObjectOfType<ClickableCharacter>().followingCharacter = true;
            FindObjectOfType<ClickableCharacter>().canMove = true;
            Camera.main.orthographicSize = 4.0f;
        }
        
        conceptualUI.transform.position = playerPos;
        conceptualUI.SetActive(show);
        conceptualUI.GetComponent<Animator>().SetBool("showMenu", show);
    }
}
