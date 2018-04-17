using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace _LetsQuiz
{
    public class FeedbackTwoButtonModal : MonoBehaviour
    {
        [Header("Component")]
        public GameObject prefab;
        public static FeedbackTwoButtonModal instance;
        private GameObject _modal;

        private static Text _heading;
        private static Text _message;
        private static Button _positiveButton;
        private static Text _positiveText;
        private static Button _negativeButton;
        private static Text _negativeText;

        private void Awake()
        {
            _modal = (GameObject)Instantiate(prefab);
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_modal);
            _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
            _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
            _positiveButton = GameObject.FindGameObjectWithTag("Modal_Positive").GetComponentInChildren<Button>();
            _positiveText = _positiveButton.GetComponentInChildren<Text>();
            _negativeButton = GameObject.FindGameObjectWithTag("Modal_Negative").GetComponentInChildren<Button>();
            _negativeText = _negativeButton.GetComponentInChildren<Text>();

            instance = this;
            instance._modal.SetActive(false);
        }

        public static void Show(string heading, string message, string postive, string negative, UnityAction positiveAction, UnityAction negativeAction, bool closeOnAction = false)
        {
            _heading.text = heading;
            _message.text = message;
            _positiveText.text = postive;
            _negativeText.text = negative;
            _positiveButton.onClick.AddListener(positiveAction);
            _negativeButton.onClick.AddListener(negativeAction);

            if (closeOnAction)
            {
                _positiveButton.onClick.AddListener(Hide);
                _negativeButton.onClick.AddListener(Hide);
            }

            instance._modal.SetActive(true);
        }

        public static void Hide()
        {
            instance._modal.SetActive(false);
        }
    }
}

