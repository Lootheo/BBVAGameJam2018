using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FinalFirebaseConnection : MonoBehaviour {

    DatabaseReference reference;
    public List<ServerData> playerData;

    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bbvajam.firebaseio.com/");
        FirebaseDatabase.DefaultInstance.GoOnline();
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //reference.ChildAdded += HandleChildAdded;
    }

    public void WriteNewUserData(string userId, ServerData data)
    {
        StartCoroutine(Waiter(userId, data));
        //reference.Child("users").Child(userId).Child("username").SetValueAsync(name);

    }

    IEnumerator Waiter(string userId, ServerData data)
    {
        yield return new WaitUntil(() => reference != null);
        string json = JsonUtility.ToJson(data);
        //Debug.Log(json);
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
        GetPlayerBases();
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

    void GetPlayerBases()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").LimitToLast(5).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                playerData = new List<ServerData>();
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot item in snapshot.Children)
                {
                    playerData.Add(JsonUtility.FromJson<ServerData>(item.GetRawJsonValue()));
                }
                // Do something with snapshot...
            }
        });
    }
}
