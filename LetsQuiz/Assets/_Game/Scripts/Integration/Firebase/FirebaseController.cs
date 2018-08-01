using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Messaging;

namespace _LetsQuiz
{
    public class FirebaseController : Singleton<FirebaseController>
    {

        #region variables

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

        private void Start()
        {
            FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;
        }

        #endregion

        #region events

        private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
        {
            FirebaseMessaging.SubscribeAsync("/topics/all");

            Token = e.Token;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Header = e.Message.Notification.Title;
            Message = e.Message.Notification.Body;
        }

        #endregion

        #region subscription

        public void ToogleSubscription(int subscribeStatus)
        {
            if (subscribeStatus == 1)
                FirebaseMessaging.SubscribeAsync("/topics/all");
            else
                FirebaseMessaging.UnsubscribeAsync("/topics/all");
        }

        #endregion

        public void CreateNotification(string token, string header, string message)
        {
            StartCoroutine(SendNotification(token, header, message));
        }

        private IEnumerator SendNotification(string token, string header, string message)
        {
            WWWForm form = new WWWForm();

            form.AddField("token", token);
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

        #endregion
    }
}

