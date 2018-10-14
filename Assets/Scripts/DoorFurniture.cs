using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorFurniture : MonoBehaviour {
	public void Activate()
    {
        SceneManager.LoadScene(3);
    }
}
