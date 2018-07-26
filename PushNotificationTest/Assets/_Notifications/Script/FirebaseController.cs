using UnityEngine;
using UnityEngine.UI;

public class FirebaseController : MonoBehaviour
{
    [Header("Compoenents")]
    public Text tokenText;
    public Text messageText;

    public void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
        tokenText.text = "Device token: " + token.Token;
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from: " + e.Message.From);
        Debug.Log("Received a new message: " + e.Message.Notification.Body);
        messageText.text = "Notification message: " + e.Message.Notification.Body;
    }
}