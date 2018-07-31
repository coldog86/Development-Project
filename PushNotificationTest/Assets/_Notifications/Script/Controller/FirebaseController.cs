using Firebase.Messaging;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Text _tokenText;
    [SerializeField] private Text _messageText;
    [SerializeField] private Text _dataText;
    [SerializeField] private Button _sendButton;
    [SerializeField] private Button _sendDelayButton;

    private float _sendTimer;
    private float _sendTimeLimit = 50000.0f;
    private bool _failed = false;

    private string _token;
    private string _message;
    private string _data;

    private bool _hasToken;
    private bool _hasMessage;

    private void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;

        _sendButton.onClick.AddListener(SendNotification);
        _sendDelayButton.onClick.AddListener(SendNotificationPost);
    }

    private void Update()
    {
        if (!_hasToken)
            SetToken();

        if (!_hasMessage)
            SetMessage();
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        FirebaseMessaging.Subscribe("/topics/all");

        _token = token.Token;
        SetToken();
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs message)
    {
        message.Message.Data.TryGetValue("dataString", out _data);

        _message = message.Message.Notification.Body;

        _dataText.text = _data;

        SetMessage();
    }

    private void SetToken()
    {
        _hasToken = false;

        if (!string.IsNullOrEmpty(_token))
        {
            _tokenText.text = _token;
            _hasToken = true;
        }
        else
            _tokenText.text = "No token recieved.";
    }

    private void SetMessage()
    {
        _hasMessage = false;

        if (!string.IsNullOrEmpty(_message))
        {
            _messageText.text = _message;
            Alert.Show(_message);
            _hasMessage = true;
        }
        else
            _messageText.text = "No message recieved.";
    }

    private void SendNotification()
    {
        Debug.Log("[FirebaseController] SendNotification()");
        WWW send = new WWW(Server.CONNECTION + Server.SEND);

        Alert.Show("Sending..", 1.0f);

        while (!send.isDone)
        {
            _sendTimer += Time.deltaTime;

            if (_sendTimer > _sendTimeLimit)
            {
                _dataText.text = "Error : Server time out.";
                _failed = true;
                break;
            }
        }

        if (!send.isDone || send.error != null)
        {
            _dataText.text = "Error : " + send.error;
        }
        else
        {
            if (!_failed)
            {
                _dataText.text = send.text;
            }
        }
    }

    private void SendNotificationPost()
    {
        Debug.Log("[FirebaseController] SendNotificationPost()");

        WWWForm notification = new WWWForm();
        notification.AddField("recipient", "/topics/all");
        notification.AddField("title", "Title set in code");
        notification.AddField("body", "Message set in code");

        WWW send = new WWW(Server.CONNECTION + Server.NOTIFICATION, notification);

        while (!send.isDone)
        {
            _sendTimer += Time.deltaTime;

            if (_sendTimer > _sendTimeLimit)
            {
                _dataText.text = "Error : Server time out.";
                _failed = true;
                break;
            }
        }

        if (!send.isDone || send.error != null)
        {
            _dataText.text = "Error : " + send.error;
        }
        else
        {
            if (!_failed)
            {
                Alert.Show("Sent");
                _dataText.text = send.text;
            }
        }
    }
}