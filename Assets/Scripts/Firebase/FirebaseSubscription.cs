using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseSubscription : MonoBehaviour {
    DatabaseReference reference;

    public DatabaseReference userReference;
    public PlayerVisitManager pvm;
    public PlayerVisitSender pvs;
    ServerData playerData;

    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bbvajam.firebaseio.com/");
        FirebaseDatabase.DefaultInstance.GoOnline();
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        pvs = GameObject.FindObjectOfType<PlayerVisitSender>();
        playerData = pvs.playerToVisit;
        if (playerData != null)
        {
            StartCoroutine(Waiter(playerData.avatarName, playerData));
        }
        //reference.ChildAdded += HandleChildAdded;
    }


    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        Debug.Log(args.Snapshot.Value.ToString());
        pvm.ConstructRoom();
        /*foreach (string item in update.Keys)
        {
            //Debug.Log(item);
        }
        foreach (object item in update.Values)
        {
            //Debug.Log(item.GetType().ToString());
        }*/
    }

    IEnumerator Waiter(string userId, ServerData data)
    {
        yield return new WaitUntil(() => reference != null);
        string json = JsonUtility.ToJson(data);
        //Debug.Log(json);
        SelectPlayer(userId);
    }

    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        //Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        //vasl = args.Snapshot.Child("miPedido").GetRawJsonValue();
        string vasl = args.Snapshot.GetRawJsonValue();
        Debug.Log(vasl);
        //mipedido = args.Snapshot.Value as PedidoClass;
        //PedidoClass mipedido = JsonUtility.FromJson<PedidoClass>(vasl);
        //mipedido.miPedido = JsonUtility.FromJson<ItemCarrito>(vasl);
        //Debug.Log(mipedido.cantidadTotal);
        //AdressData data = args.Snapshot.Child("direccionEnvio").Value as AdressData;

        //topScore = (long)update["topScore"];
        //Debug.Log("NewTopScore:" + topScore);
    }

    void HandleChildChanged(object sender, ChildChangedEventArgs args)
    {
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        //string msg = update["message"].ToString();
        //string mail = update["email"].ToString();
        string time = update["timeString"].ToString();
        Debug.Log(time);
        Debug.Log("Estamos en child changed");
        Debug.Log(args.Snapshot.Key);
        Debug.Log(args.Snapshot.Reference);
    }

    public void SelectPlayer(string username)
    {
        userReference = FirebaseDatabase.DefaultInstance.GetReference("users").Child(username);
        userReference.ValueChanged += HandleValueChanged;
        userReference.ChildChanged += HandleChildChanged;
    }
}
