using Firebase.Messaging;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class FirebaseController : Singleton<FirebaseController>
    {
        #region variables

        [Header("Components")]
        [SerializeField] private Text _tokenText;

        [HideInInspector]
        public string Token;
        private float _connectionTimer = 0.0f;
        private const float _connectionTimeLimit = 10000.0f;

        #endregion variables

        #region properties

        public string Header { get; private set; }
        public string Message { get; private set; }

        #endregion properties

        #region methods

        #region unity

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            FirebaseMessaging.TokenReceived -= OnTokenReceived;
            FirebaseMessaging.MessageReceived -= OnMessageReceived;
        }

        #endregion unity

        #region events

        private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
        {
            Debug.Log("[FirebaseController] OnTokenRecieved() Token : " + e.Token);

            Token = e.Token;

            FirebaseMessaging.SubscribeAsync(Token);
            FirebaseMessaging.SubscribeAsync("/topics/all");

            PlayerController.Instance.SetToken(Token);

            if (!string.IsNullOrEmpty(Token))
                _tokenText.text = Token;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.Log("[FirebaseController] OnMessageReceived() Notification received from : " + e.Message.From + " with title : " + e.Message.Notification.Title + " with messgae : " + e.Message.Notification.Body);

            Header = e.Message.Notification.Title;
            Message = e.Message.Notification.Body;
        }

        #endregion events

        #region notification

        public void CreateNotification(string token, string header, string message)
        {
            Debug.LogFormat("[{0}] CreateNotification() : \nToken {1}\nHeading {2}\nMessage {3}", GetType().Name, token, header, message);

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(message))
                StartCoroutine(SendNotification(token, header, message));
        }

        private IEnumerator SendNotification(string token, string header, string message)
        {
            Debug.LogFormat("[{0}] SendNotification() : \nToken {1}\nHeading {2}\nMessage {3}", GetType().Name, token, header, message);

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

        #endregion notification

        #region notification - debug

        public void CreateDebugNotification(string token, string header, string message)
        {
            if (!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(message))
                StartCoroutine(SendDebugNotification(token, header, message));
        }

        private IEnumerator SendDebugNotification(string token, string header, string message)
        {
            WWWForm form = new WWWForm();
            form.AddField("token", token);
            form.AddField("title", header);
            form.AddField("body", message);

            WWW notificationRequest = new WWW(ServerHelper.Host + ServerHelper.SendDebugNotification, form);

            _connectionTimer += Time.deltaTime;

            while (!notificationRequest.isDone)
            {
                if (_connectionTimer > _connectionTimeLimit)
                {
                    Debug.LogError("[FirebaseController] SendDebugNotification() : " + notificationRequest.error);
                    yield return null;
                }
                else if (notificationRequest.error != null)
                {
                    Debug.Log("[FirebaseController] SendDebugNotification() : " + notificationRequest.error);
                    yield return null;
                }
                // extra check just to ensure a stream error doesn't come up
                else if (_connectionTimer > _connectionTimeLimit && notificationRequest.error != null)
                {
                    Debug.LogError("[FirebaseController] SendDebugNotification() : " + notificationRequest.error);
                    yield return null;
                }
            }

            if (notificationRequest.isDone && notificationRequest.error != null)
            {
                Debug.Log("[FirebaseController] SendDebugNotification() : " + notificationRequest.error);
                yield return null;
            }

            if (notificationRequest.isDone)
            {
                // check that the notification request returned something
                if (!string.IsNullOrEmpty(notificationRequest.text))
                {
                    Debug.Log("[FirebaseController] SendDebugNotification() : " + notificationRequest.text);
                    yield return notificationRequest;
                }
            }
        }

        #endregion notification - debug

        #region database

        public void InsertToken(int userId, string username)
        {
            StartCoroutine(Insert(userId, username));
        }

        private IEnumerator Insert(int userId, string username)
        {
            Debug.LogFormat("[{0}] Insert() Id {1} Token {2}", GetType().Name, userId, Token);

            var form = new WWWForm();
            form.AddField("userId", userId);
            form.AddField("username", username);
            form.AddField("token", Token);

            WWW request;

            if (!PlayerPrefs.HasKey(DataHelper.PlayerDataKey.TOKEN))
                request = new WWW(ServerHelper.Host + ServerHelper.FirebaseTokenInsert, form);
            else if (Token != PlayerController.Instance.GetToken())
                request = new WWW(ServerHelper.Host + ServerHelper.FirebaseTokenUpdate, form);
            else
                request = new WWW(ServerHelper.Host + ServerHelper.FirebaseTokenUpdate, form);

            _connectionTimer += Time.deltaTime;

            while (!request.isDone)
            {
                if (_connectionTimer > _connectionTimeLimit)
                    yield return null;

                if (request.error != null)
                    yield return null;

                if (_connectionTimer > _connectionTimeLimit && request.error != null)
                    yield return null;
            }

            if (request.isDone && request.error != null)
                yield return null;

            if (request.isDone)
                yield return request.text;
        }

        public string SelectToken(int userId)
        {
            return Select(userId);
        }

        private string Select(int userId)
        {
            _connectionTimer = 0.0f;
            var token = "";
            var form = new WWWForm();
            form.AddField("userId", userId);
            form.AddField("username", PlayerController.Instance.GetUsername());

            var request = new WWW(ServerHelper.Host + ServerHelper.FirebaseTokenSelect, form);

            _connectionTimer += Time.deltaTime;

            while (!request.isDone)
            {
                if (_connectionTimer > _connectionTimeLimit)
                    token = "";

                if (request.error != null)
                    token = "";

                if (_connectionTimer > _connectionTimeLimit && request.error != null)
                    token = "";
            }

            if (request.isDone && request.error != null)
                token = "";

            if (request.isDone)
            {
                token = request.text;
            }

            return token;
        }

        #endregion database

        #endregion methods
    }
}