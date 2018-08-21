using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class NotificationHelper : MonoBehaviour
    {
        [Header("Notification")]
        public InputField notifcationTitleInput;
        public InputField notifcationBodyInput;

        public void SendDebugNotification()
        {
            var header = notifcationTitleInput.text;
            var message = notifcationBodyInput.text;

            if (string.IsNullOrEmpty(header))
                header = "Notification Header";

            if (string.IsNullOrEmpty(message))
                header = "Notification Message";

            FirebaseController.Instance.CreateDebugNotification(header, message);
        }
    }
}