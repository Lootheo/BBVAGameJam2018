using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class TestConnection : MonoBehaviour
{

    Firebase.Auth.FirebaseAuth auth;
    public DatabaseReference baseRef;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    public Text logText;
    public string uid;
    public GameObject container;
    public GameObject peticionPref;
    public List<GameObject> peticionesTotales;
    public float currentLocation;
    public int ID_Sucursal;
    public AudioSource source;
    public AudioClip bell;

    public void SetSucursal(int _dex)
    {
        currentLocation = 0f;
        ID_Sucursal = _dex;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bbvajam.firebaseio.com/");
        //dbr = FirebaseDatabase.DefaultInstance.GetReference("scores");
        baseRef = FirebaseDatabase.DefaultInstance.GetReference("Pedidos").Child("Sucursal_" + ID_Sucursal);
        SetParameters();
        //dbr.ValueChanged += HandleTopScoreChange;
        //baseRef.Child(uid).Child(selectedPlayer.uid).ChildChanged += HandleMessageEdited;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SetSucursal(2);
            //AddMessage();
            writeNewUser("12312312", "Juan", "Test@mail.com");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void GetMessages()
    {
        /*baseRef.Child("testUser1").GetValueAsync().ContinueWith(task => 
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                Dictionary<string, object> messages = (Dictionary<string, object>)task.Result.Value;
                Debug.Log(messages["message"]);
            }
        });*/
        /*dbr.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                Dictionary<string, object> messages = (Dictionary<string, object>)task.Result.Value;
                topScore = (long)messages["topScore"];
            }
        });*/
    }

    public void AddMessage()
    {
        //Pedido newMsg = new Pedido("55555", "Test Pedido 1");
        //string msgJSON = JsonUtility.ToJson(newMsg);

        baseRef.Child("Pedido"+1).SetRawJsonValueAsync("Test message");

        DatabaseReference newPostRef = baseRef.Push();
        string postId = newPostRef.Key;
        //Debug.Log(postId);
        newPostRef.SetRawJsonValueAsync("Test message");
    }

    void HandleMessageEdited(object sender, ChildChangedEventArgs args)
    {
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        //string msg = update["message"].ToString();
        //string mail = update["email"].ToString();
        //logText.text += mail + ": " + msg + "\n";
        //topScore = (long)update["topScore"];
        //Debug.Log("NewTopScore:" + topScore);
    }

    void SetParameters()
    {
        DateTime date = DateTime.Now;
        Debug.Log(date.ToShortDateString());
        Debug.Log(date.ToShortTimeString());
        Debug.Log(date.ToString("D"));
        Debug.Log(date.ToString("F"));
        Debug.Log(date.ToString("f"));
        Debug.Log(date.ToString("G"));
        baseRef.ChildAdded += HandleChildAdded;
        baseRef.ValueChanged += HandleValueChanged;
        baseRef.ChildChanged += HandleChildChanged;
        baseRef.ChildRemoved += HandleChildRemoved;
        Debug.Log(baseRef.ToString());
        Debug.Log(baseRef.Root.ToString());
        Debug.Log(baseRef.Parent.ToString());
        //GetMessages();
        //GetMessages();
    }

    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        //Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        //vasl = args.Snapshot.Child("miPedido").GetRawJsonValue();
        string vasl = args.Snapshot.GetRawJsonValue();
        //mipedido = args.Snapshot.Value as PedidoClass;
        //PedidoClass mipedido = JsonUtility.FromJson<PedidoClass>(vasl);
        //mipedido.miPedido = JsonUtility.FromJson<ItemCarrito>(vasl);
        //Debug.Log(mipedido.cantidadTotal);
        //AdressData data = args.Snapshot.Child("direccionEnvio").Value as AdressData;

        //Debug.Log(data.Fraccionamiento);
        //AdressData newData = JsonUtility.FromJson<AdressData>(update["direccionEnvio"].ToString());
        //Debug.Log(newData.Colonia);
        //Debug.Log(temp);
        //AdressData newData = JsonUtility.FromJson<AdressData>(temp);
        Debug.Log("Estamos en child added");
        //selectedChild = args.Snapshot.Key;
        //Debug.Log(selectedChild);
        GameObject newOrder = Instantiate(peticionPref) as GameObject;
        peticionesTotales.Add(newOrder);
        //PeticionScript ps = newOrder.GetComponent<PeticionScript>();
        //ps.SetPetition(args.Snapshot.Key, this, mipedido);
        newOrder.transform.SetParent(container.transform);
        RectTransform currRect = newOrder.GetComponent<RectTransform>();
        currRect.anchoredPosition = new Vector2(5, -currentLocation - 5);
        currRect.sizeDelta = new Vector2(-10, currRect.sizeDelta.y);
        currentLocation += currRect.sizeDelta.y + 5;
        currRect.localScale = new Vector3(1, 1, 1);
        container.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 5 + currentLocation);
        source.PlayOneShot(bell);
        //topScore = (long)update["topScore"];
        //Debug.Log("NewTopScore:" + topScore);
    }

    void HandleChildRemoved(object sender, ChildChangedEventArgs args)
    {
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        //string msg = update["message"].ToString();
        //string mail = update["email"].ToString();
        Debug.Log("Estamos en child removed");
    }

    string selectedChild;

    void HandleChildChanged(object sender, ChildChangedEventArgs args)
    {
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        //string msg = update["message"].ToString();
        //string mail = update["email"].ToString();
        string time = update["timeString"].ToString();
        Debug.Log(time);
        Debug.Log("Estamos en child changed");
        Debug.Log(args.Snapshot.Key);
        selectedChild = args.Snapshot.Key;
        Debug.Log(args.Snapshot.Reference);
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;

        /*foreach (string item in update.Keys)
        {
            //Debug.Log(item);
        }
        foreach (object item in update.Values)
        {
            //Debug.Log(item.GetType().ToString());
        }*/
    }

    public void RemoveLastEntry()
    {
        baseRef.Child(selectedChild).RemoveValueAsync();
    }


    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    /*protected virtual void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
        topicField.text = topic;
    }*/


    // Setup message event handlers.
    void InitializeFirebase()
    {
        //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        //Firebase.Messaging.FirebaseMessaging.Subscribe(topic);
        DebugLog("Firebase Messaging Initialized");
    }

    /*public virtual void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        string mess = "";
        mess += "Received a new message\n";
        var notification = e.Message.Notification;
        if (notification != null)
        {
            mess += "title: " + notification.Title + "\n";
            mess += "body: " + notification.Body + "\n";
        }
        if (e.Message.From.Length > 0)
            mess += "from: " + e.Message.From + "\n";
        if (e.Message.Link != null)
        {
            mess += "link: " + e.Message.Link.ToString() + "\n";
        }
        if (e.Message.Data.Count > 0)
        {
            mess += "data:" + "\n";
            foreach (System.Collections.Generic.KeyValuePair<string, string> iter in e.Message.Data)
            {
                mess += "  " + iter.Key + ": " + iter.Value + "\n";
            }
        }
        DebugLog(mess);
    }*/

    /*public virtual void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        DebugLog("Received Registration Token: " + token.Token);
    }*/


    // End our messaging session when the program exits.
    public void OnDestroy()
    {
        //Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
        //Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
    }

    public void DisconnectFromFirebase()
    {
        ClearEntrys();
        baseRef.ChildAdded -= HandleChildAdded;
        baseRef.ValueChanged -= HandleValueChanged;
        baseRef.ChildChanged -= HandleChildChanged;
        baseRef.ChildRemoved -= HandleChildRemoved;
    }

    void ClearEntrys()
    {
        foreach (GameObject item in peticionesTotales)
        {
            Destroy(item);
        }
        peticionesTotales.Clear();
        container.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 5);
        currentLocation = 0;
    }

    DatabaseReference reference;
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bbvajam.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void writeNewUser(string userId, string name, string email)
    {
        User user = new User(name, email);
        string json = JsonUtility.ToJson(user);

        reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
        //reference.Child("users").Child(userId).Child("username").SetValueAsync(name);

    }

    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s)
    {
        logText.text = s;
    }
}

public class User
{
    public string username;
    public string email;

    public User()
    {
    }

    public User(string username, string email)
    {
        this.username = username;
        this.email = email;
    }
}
