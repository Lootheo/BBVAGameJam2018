using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisitSender : MonoBehaviour {
    public ServerData playerToVisit;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        //GameObject.FindObjectOfType<>
	}
}
