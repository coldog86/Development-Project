using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Messaging;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class FirebaseController : Singleton<FirebaseController>
    {

        #region variables

        [SerializeField] private Text _tokenText;
        private float _connectionTimer = 0.0f;
        private const float _connectionTimeLimit = 10000.0f;

        #endregion

        public string Token { get; private set; }

        public string Header { get; private set; }

        public string Message { get; private set; }

        #region properties

        #endregion

        #region methods

        #region unity

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;
        }

        #endregion

        #region events

        private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
        {
            Debug.Log("[FirebaseController] OnTokenRecieved() Token : " + e.Token);

            FirebaseMessaging.SubscribeAsync("/topics/all");

            Token = e.Token;

            if (!string.IsNullOrEmpty(Token))
                _tokenText.text = Token;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.Log("[FirebaseController] OnMessageReceived() Notification received from : " + e.Message.From + " with title : " + e.Message.Notification.Title + " with messgae : " + e.Message.Notification.Body);
            Header = e.Message.Notification.Title;
            Message = e.Message.Notification.Body;
        }

        #endregion

        #region subscription

        public void ToogleSubscription(bool subscribeStatus)
        {
            if (subscribeStatus)
            {
                FirebaseMessaging.SubscribeAsync("/topics/all");
                FirebaseMessaging.SubscribeAsync(Token);
            }
            else
            {
                FirebaseMessaging.UnsubscribeAsync("/topics/all");
                FirebaseMessaging.UnsubscribeAsync(Token);
            }
        }

        #endregion

        public void CreateNotification(string token, string header, string message)
        {
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(message))
                StartCoroutine(SendNotification(token, header, message));
        }

        public void CreateDebugNotification(string header, string message)
        {
            if (!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(message))
                StartCoroutine(SendDebugNotification(header, message));
        }

        private IEnumerator SendNotification(string token, string header, string message)
        {
            WWWForm form = new WWWForm();

            form.AddField("recipient", token);
            form.AddField("title", header);
            form.AddField("body", message);

            WWW notificationRequest = new WWW(ServerHelper.Host + ServerHelper.SendNotification, form);

            _connectionTimer += Time.deltaTime;

            while (!notificationRequest.isDone)
            {
                if (_connectionTimer > _connectionTimeLimit)
                {
                    Debug.LogError("[FirebaseController] SendNotification() : " + notificationRequest.error);
                    yield return null;
                }
                else if (notificationRequest.error != null)
                {
                    Debug.Log("[FirebaseController] SendNotification() : " + notificationRequest.error);
                    yield return null;
                }
                // extra check just to ensure a stream error doesn't come up
                else if (_connectionTimer > _connectionTimeLimit && notificationRequest.error != null)
                {
                    Debug.LogError("[FirebaseController] SendNotification() : " + notificationRequest.error);
                    yield return null;
                }
            }

            if (notificationRequest.isDone && notificationRequest.error != null)
            {
                Debug.Log("[FirebaseController] SendNotification() : " + notificationRequest.error);
                yield return null;
            }

            if (notificationRequest.isDone)
            {
                // check that the notification request returned something
                if (!string.IsNullOrEmpty(notificationRequest.text))
                {
                    Debug.Log("[FirebaseController] SendNotification() : " + notificationRequest.text);
                    yield return notificationRequest;
                }
            }

        }

        private IEnumerator SendDebugNotification(string header, string message)
        {
            WWWForm form = new WWWForm();

            form.AddField("title", header);
            form.AddField("body", message);

            WWW notificationRequest = new WWW(ServerHelper.Host + ServerHelper.SendDebugNotification, form);

            _connectionTimer += Time.deltaTime;

            while (!notificationRequest.isDone)
            {
                if (_connectionTimer > _connectionTimeLimit)
                {
                    Debug.LogError("[FirebaseController] SendNotification() : " + notificationRequest.error);
                    yield return null;
                }
                else if (notificationRequest.error != null)
                {
                    Debug.Log("[FirebaseController] SendNotification() : " + notificationRequest.error);
                    yield return null;
                }
                // extra check just to ensure a stream error doesn't come up
                else if (_connectionTimer > _connectionTimeLimit && notificationRequest.error != null)
                {
                    Debug.LogError("[FirebaseController] SendNotification() : " + notificationRequest.error);
                    yield return null;
                }
            }

            if (notificationRequest.isDone && notificationRequest.error != null)
            {
                Debug.Log("[FirebaseController] SendNotification() : " + notificationRequest.error);
                yield return null;
            }

            if (notificationRequest.isDone)
            {
                // check that the notification request returned something
                if (!string.IsNullOrEmpty(notificationRequest.text))
                {
                    Debug.Log("[FirebaseController] SendNotification() : " + notificationRequest.text);
                    yield return notificationRequest;
                }
            }
        }

        #endregion
    }
}