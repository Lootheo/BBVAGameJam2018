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
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
