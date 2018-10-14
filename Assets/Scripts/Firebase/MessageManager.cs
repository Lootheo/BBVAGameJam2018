using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MessageManager : MonoBehaviour
{
    long topScore;
    long currScore;
    public GameObject rowPrefab;
    public GameObject scrollContainer;
    Firebase.Auth.FirebaseAuth auth;
    public GameObject profilePanel;
    DatabaseReference dbr;
    DatabaseReference baseRef;
    public delegate void ScoreAction();
    public static event ScoreAction TopScoreUpdated;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    private string topic = "TestTopic";
    public Button subBtn;
    public Button unSubBtn;
    public Text logText;
    public InputField topicField;
    public InputField messageField;
    public Animator anim;
    public Player selectedPlayer;
    public string uid;

    void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bbvajam.firebaseio.com/");
        dbr = FirebaseDatabase.DefaultInstance.GetReference("scores");
        baseRef = FirebaseDatabase.DefaultInstance.GetReference("messages");

        dbr.ValueChanged += HandleTopScoreChange;
        //selectedPlayer = cc.selectedPlayer;



        //baseRef.Child(uid).Child(selectedPlayer.uid).ChildChanged += HandleMessageEdited;
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
        baseRef.Child(selectedPlayer.uid).Child(uid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Working here too");
                DataSnapshot history = task.Result;
                Debug.Log(task.Result.ToString());
                foreach (DataSnapshot messageNode in history.Children)
                {
                    //var playerDictionary = (IDictionary<String, object>)messageNode.Value;
                    //Message newMessage = new Message(playerDictionary);
                    //messageList.Add(newMessage);
                    //CreateRowMessage(newMessage);
                    _messageIndex++;
                }
            }
        });
        dbr.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                Dictionary<string, object> messages = (Dictionary<string, object>)task.Result.Value;
                topScore = (long)messages["topScore"];
            }
        });
    }

    public int _messageIndex = 0;

    public void AddMessage()
    {
        Message newMsg = new Message("FenrirUser", "Hello from firebase");
        string msgJSON = JsonUtility.ToJson(newMsg);
        //baseRef.Child(uid).Child(selectedPlayer.uid).Child("Message"+_messageIndex).SetRawJsonValueAsync(msgJSON);
        baseRef.Child("Temp").Child("User").Child("Message" + _messageIndex).SetRawJsonValueAsync(msgJSON);
        //logText.text += uid + ": " + messageField.text + "\n";
        //messageField.text = "";
        _messageIndex++;
    }

    public void CloseChat()
    {
        anim.SetBool("closeChat", true);
        StartCoroutine(closeSceneAsync());
    }

    IEnumerator closeSceneAsync()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 2.0f);
        SceneManager.UnloadSceneAsync("Chat");
    }

    public void LoadMessages()
    {

    }

    public void LogScore(long s)
    {
        currScore = s;
        if (currScore > topScore)
        {
            dbr.RunTransaction(UpdateScore);
        }

    }

    TransactionResult UpdateScore(MutableData nd)
    {
        if (nd.Value != null)
        {
            Dictionary<string, object> updatedScore = nd.Value as Dictionary<string, object>;
            topScore = (long)updatedScore["topScore"];
        }
        if (currScore > topScore)
        {
            topScore = currScore;
            nd.Value = new Dictionary<string, object>() { { "topScore", currScore } };
            return TransactionResult.Success(nd);
        }
        return TransactionResult.Abort();
    }

    void HandleTopScoreChange(object sender, ValueChangedEventArgs args)
    {
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        topScore = (long)update["topScore"];
        Debug.Log("NewTopScore:" + topScore);
        if (TopScoreUpdated != null)
            TopScoreUpdated();
    }

    void HandleNewMessage(object sender, ChildChangedEventArgs args)
    {
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        string msg = update["message"].ToString();
        string mail = update["email"].ToString();
        logText.text += mail + ": " + msg + "\n";
        //topScore = (long)update["topScore"];
        //Debug.Log("NewTopScore:" + topScore);
    }

    void HandleMessageEdited(object sender, ChildChangedEventArgs args)
    {
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        string msg = update["message"].ToString();
        string mail = update["email"].ToString();
        logText.text += mail + ": " + msg + "\n";
        //topScore = (long)update["topScore"];
        //Debug.Log("NewTopScore:" + topScore);
    }

    void Start()
    {
        DateTime date = DateTime.Now;
        Debug.Log(date.ToShortDateString());
        Debug.Log(date.ToShortTimeString());
        Debug.Log(date.ToString("D"));
        Debug.Log(date.ToString("F"));
        Debug.Log(date.ToString("f"));
        Debug.Log(date.ToString("G"));
        uid = "Az0Dd8j2dVajIwgYilUe5ErkYp02";
        //uid = "Az0Dd8j2dVajIwgYilUe5ErkYp02";

        baseRef.Child(uid).Child(selectedPlayer.uid).ChildAdded += HandleNewMessage;
        GetMessages();
        //GetMessages();
    }

    void InitializeUI()
    {
        /*foreach (Message player in messageList)
        {
            CreateRowMessage(player);
        }*/

    }

    void CreateRowMessage(Message player)
    {
        GameObject newRow = Instantiate(rowPrefab) as GameObject;
        newRow.GetComponentInChildren<Text>().text = player.message;
        newRow.transform.SetParent(scrollContainer.transform, false);
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

    // Exit if escape (or back, on mobile) is pressed.
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddMessage();
        }
    }

    // End our messaging session when the program exits.
    /*public void OnDestroy()
    {
        Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
    }*/

    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s)
    {
        logText.text = s;
    }


    public void Subscribe()
    {
        topic = topicField.text;
        //Firebase.Messaging.FirebaseMessaging.Subscribe(topic);
        DebugLog("Subscribed to " + topicField.text);
    }

    public void Unsubscribe()
    {
        topic = topicField.text;
        //Firebase.Messaging.FirebaseMessaging.Unsubscribe(topic);
        DebugLog("Unsubscribed to " + topicField.text);
    }
}

[System.Serializable]
public class Player
{
    public string email;
    public int score;
    public int level;
    public string uid;

    public Player(string email, int score, int level, string key)
    {
        this.email = email;
        this.score = score;
        this.level = level;
        uid = key;
    }

    public Player(IDictionary<string, object> dict, string key)
    {
        this.email = dict["email"].ToString();
        this.score = Convert.ToInt32(dict["score"]);
        this.level = Convert.ToInt32(dict["level"]);
        uid = key;
    }
}

[System.Serializable]
public class Message
{
    public string timeString;
    public string email;
    public string message;


    public Message(string email, string message)
    {
        timeString = DateTime.Now.ToString("G");
        this.email = email;
        this.message = message;

    }

    public Message(IDictionary<string, object> dict)
    {
        timeString = DateTime.Now.ToString("G");
        this.email = dict["email"].ToString();
        this.message = dict["message"].ToString();

    }
}
