using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class NotificationHelper : MonoBehaviour
    {
        [Header("Notification")]
        public InputField notifcationTitleInput;
        public InputField notifcationBodyInput;

        [SerializeField] private string _token;

        private float _connectionTimer = 0.0f;
        private float _connectionTimeLimit = 100000.0f;

        public void SendNotification()
        {
            _token = FirebaseController.Instance.SelectToken(PlayerController.Instance.GetId());

            var header = notifcationTitleInput.text;
            var message = notifcationBodyInput.text;

            if (string.IsNullOrEmpty(header))
                header = "Notification Header";

            if (string.IsNullOrEmpty(message))
                message = "Notification Message";

            Debug.LogFormat("[{0}] SendNotification() \nToken {1}\nHeading {2} \nMessage {3}", GetType().Name, _token, header, message);

            FirebaseController.Instance.CreateNotification(_token, header, message);
        }

        public void SendDebugNotification()
        {
            _token = FirebaseController.Instance.SelectToken(PlayerController.Instance.GetId());

            var header = notifcationTitleInput.text;
            var message = notifcationBodyInput.text;

            if (string.IsNullOrEmpty(header))
                header = "Notification Header";

            if (string.IsNullOrEmpty(message))
                message = "Notification Message";

            Debug.LogFormat("[{0}] SendDebugNotification() \nToken {1}\nHeading {2} \nMessage {3}", GetType().Name, _token, header, message);

            FirebaseController.Instance.CreateDebugNotification(_token, header, message);
        }

        public void SelectToken(int userId)
        {
            StartCoroutine(Select(userId));
        }

        private IEnumerator Select(int userId)
        {
            _connectionTimer = 0.0f;

            var form = new WWWForm();
            form.AddField("userId", userId);

            var request = new WWW(ServerHelper.Host + ServerHelper.FirebaseTokenSelect, form);

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
            {
                yield return request.text;
                Debug.Log(request.text);
            }
        }
    }
}