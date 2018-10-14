using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseConnection : MonoBehaviour {
    public DatabaseReference baseRef;
    public int ID_Sucursal;

    // Use this for initialization
    void OnEnable()
    {
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bbvajam.firebaseio.com/");
        FirebaseDatabase.DefaultInstance.GoOnline();
        //GetMessages();
        //GetMessages();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddMessage(2);
        }
    }

    public void AddMessage(int _id)
    {
        ID_Sucursal = _id;
        baseRef = FirebaseDatabase.DefaultInstance.GetReference("Pedidos").Child("Sucursal_" + ID_Sucursal);
        string msgJSON = "This is a test message";
        
        baseRef.Child("Pedido"+12).SetRawJsonValueAsync(msgJSON);
        DatabaseReference newPostRef = baseRef.Push();
        string postId = newPostRef.Key;
        Debug.Log(postId);
        newPostRef.SetRawJsonValueAsync(msgJSON);
    }
}
