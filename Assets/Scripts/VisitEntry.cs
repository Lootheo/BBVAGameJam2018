using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisitEntry : MonoBehaviour {
    public Text hostUsername;
    public ServerData hostData;
    public Button visitBtn;

    public void SetData(ServerData host)
    {
        hostUsername.text = host.avatarName;
        hostData = host;
    }
}
