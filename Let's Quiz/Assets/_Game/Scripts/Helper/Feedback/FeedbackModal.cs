using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace _LetsQuiz
{
    public class FeedbackModal : MonoBehaviour
    {
        [Header("Component")]
        public GameObject oneButtonPrefab;
        public GameObject twoButtonPrefab;

        private GameObject _modal;
        private Text _heading;
        private Text _message;
        private Text _action;
        private Text _negative;
        private Text _positive;

        [HideInInspector]
        public Button actionButton;
        [HideInInspector]
        public Button negativeButton;
        [HideInInspector]
        public Button positiveButton;

        private FeedbackClick _click;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            _click = FindObjectOfType<FeedbackClick>();
        }

        private void Initialise(bool oneButton)
        {
            if (oneButton)
            {
                _modal = (GameObject)Instantiate(oneButtonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
                _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
                actionButton = GameObject.FindGameObjectWithTag("Modal_Action").GetComponentInChildren<Button>();
                _action = actionButton.GetComponentInChildren<Text>();
                actionButton.onClick.AddListener(Hide);
                actionButton.onClick.AddListener(_click.HandleClick);
            }
            else
            {
                _modal = (GameObject)Instantiate(twoButtonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
                _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
                negativeButton = GameObject.FindGameObjectWithTag("Modal_Negative").GetComponentInChildren<Button>();
                _negative = negativeButton.GetComponentInChildren<Text>();
                negativeButton.onClick.AddListener(Hide);
                negativeButton.onClick.AddListener(_click.HandleClick);
                positiveButton = GameObject.FindGameObjectWithTag("Modal_Positive").GetComponentInChildren<Button>();
                _positive = positiveButton.GetComponentInChildren<Text>();
                positiveButton.onClick.AddListener(Hide);
                positiveButton.onClick.AddListener(_click.HandleClick);
            }
        }

        public void Show(bool oneButton, string heading, string message, string action = null, string negative = null, string positive = null)
        {
            Initialise(oneButton);

            _modal.SetActive(true);
            _heading.text = heading;
            _message.text = message;

            if (oneButton)
                _action.text = action;
            else
            {
                _negative.text = negative;
                _positive.text = positive;
            }
        }

        private void Hide()
        {
            _modal.SetActive(false);
            if (!_modal.activeSelf)
                Destroy(_modal);
        }
    }
}