using Firebase.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseController : MonoBehaviour
{
    [Header("Compoenents")]
    [SerializeField] private string _token;
    [SerializeField] private string _message;
    public Text tokenText;
    public Text messageText;

    private bool _hasToken;
    private bool _hasMessage;

    private void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
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
        Debug.Log("Received Registration Token: " + token.Token);
        _token = token.Token;
        SetToken();
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from: " + e.Message.From);
        _message = e.Message.Notification.Body;
        SetMessage();
    }

    private void SetToken()
    {
        _hasToken = false;
        if (!string.IsNullOrEmpty(_token))
        {
            tokenText.text = _token;
            _hasToken = true;
        }
    }

    private void SetMessage()
    {
        _hasMessage = false;
        if (!string.IsNullOrEmpty(_message))
        {
            messageText.text = _message;
            _hasMessage = true;
        }
    }
}