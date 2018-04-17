using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace _LetsQuiz
{
    public class FeedbackOneButtonModal : MonoBehaviour
    {
        [Header("Component")]
        public GameObject prefab;
        public static FeedbackOneButtonModal instance;
        private GameObject _modal;

        private static Text _heading;
        private static Text _message;
        private static Button _actionButton;
        private static Text _actionText;

        private void Awake()
        {
            _modal = (GameObject)Instantiate(prefab);
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_modal);
            _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
            _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
            _actionButton = GameObject.FindGameObjectWithTag("Modal_Action").GetComponentInChildren<Button>();
            _actionText = _actionButton.GetComponentInChildren<Text>();

            instance = this;
            instance._modal.SetActive(false);
        }

        public static void Show(string heading, string message, string action, UnityAction eventToAction, bool closeOnAction = false)
        {
            _heading.text = heading;
            _message.text = message;
            _actionText.text = action;
            _actionButton.onClick.AddListener(eventToAction);

            if (closeOnAction)
                _actionButton.onClick.AddListener(Hide);

            instance._modal.SetActive(true);
        }

        public static void Hide()
        {
            instance._modal.SetActive(false);
        }
    }
}

