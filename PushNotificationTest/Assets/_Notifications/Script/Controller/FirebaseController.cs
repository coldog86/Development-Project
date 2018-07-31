using Firebase.Messaging;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Text _headerText;
    [SerializeField] private Text _messageText;
    [SerializeField] private InputField _headerInput;
    [SerializeField] private InputField _messageInput;
    [SerializeField] private Button _sendButton;
    [SerializeField] private Button _sendDelayButton;

    private float _sendTimer;
    private const float _sendTimeLimit = 50000.0f;
    private bool _failed = false;

    private string _token;
    private string _header;
    private string _message;

    private bool _hasMessage;

    private void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;

        _sendButton.onClick.AddListener(SendNotification);
        _sendDelayButton.onClick.AddListener(GetNotificationContent);
    }

    private void Update()
    {
        if (!_hasMessage)
            SetMessage();
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        FirebaseMessaging.SubscribeAsync("/topics/all");

        _token = token.Token;

        Alert.Show("Token Recieved");
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs message)
    {
        _header = message.Message.Notification.Title;
        _message = message.Message.Notification.Body;
        SetMessage();
    }

    private void SetMessage()
    {
        _hasMessage = false;

        if (!string.IsNullOrEmpty(_message))
        {
            _headerText.text = _header;
            _messageText.text = _message;
            _hasMessage = true;
        }
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
                _failed = true;
                break;
            }
        }

        if (!send.isDone || send.error != null)
        {
            _failed = true;
        }
        else
        {
            if (!_failed)
                _messageText.text = send.text;
        }
    }

    private void GetNotificationContent()
    {
        _header = _headerInput.text;
        _message = _messageInput.text;

        if (string.IsNullOrEmpty(_header))
            _header = "empty header";

        if (string.IsNullOrEmpty(_message))
            _message = "empty message";

        SendNotificationPost(_token, _header, _message);
    }

    private void SendNotificationPost(string token, string header, string message)
    {
        Debug.Log("[FirebaseController] SendNotificationPost()");

        WWWForm notification = new WWWForm();
        notification.AddField("recipient", token);
        notification.AddField("title", header);
        notification.AddField("body", message);

        WWW send = new WWW(Server.CONNECTION + Server.NOTIFICATION, notification);

        while (!send.isDone)
        {
            _sendTimer += Time.deltaTime;

            if (_sendTimer > _sendTimeLimit)
            {
                Alert.Show("Server Time Out.");
                Debug.Log("[FirebaseController] SendNotificationPost() : Server time out.");
                _messageText.text = "";
                _failed = true;
                break;
            }
            else if (send.error != null)
            {
                Alert.Show("Server Error.");
                Debug.Log("[FirebaseController] SendNotificationPost() : Server error.");
                _messageText.text = "";
                break;
            }
            else if (_sendTimer > _sendTimeLimit && send.error != null)
            {
                Alert.Show("Critical Error.");
                Debug.Log("[FirebaseController] SendNotificationPost() : Critical error.");
                _messageText.text = "";
                break;
            }
        }

        if (!send.isDone || send.error != null)
        {
            _messageText.text = "";
        }
        else
        {
            if (!_failed)
            {
                Alert.Show("Sent");
                _messageText.text = send.text;
            }
        }
    }
}