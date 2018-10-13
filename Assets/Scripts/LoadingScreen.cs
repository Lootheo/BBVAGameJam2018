using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerData loadedPlayer = SaveData.Load();
        if (loadedPlayer.avatarName != "")
        {
            SceneManager.LoadScene("CharacterCreation");
        }
        else
        {
            SceneManager.LoadScene("CharacterCreation");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
