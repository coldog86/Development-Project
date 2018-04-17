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
            // keeps object around so that modal can be called whenever
            DontDestroyOnLoad(gameObject);

            // finds the click helper for audio response
            _click = FindObjectOfType<FeedbackClick>();
        }

        private void Initialise(bool oneButton)
        {
            // determines if one or two button modal needs to be instaniated
            if (oneButton)
            {
                // instantiate the modal
                _modal = Instantiate(oneButtonPrefab);

                // find the required components
                _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
                _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
                actionButton = GameObject.FindGameObjectWithTag("Modal_Action").GetComponentInChildren<Button>();
                _action = actionButton.GetComponentInChildren<Text>();

                // add onclick listeners
                actionButton.onClick.AddListener(Hide);
                actionButton.onClick.AddListener(_click.Play);
            }
            else
            {
                // instantiate the modal
                _modal = Instantiate(twoButtonPrefab);

                // find the required components
                _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
                _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
                negativeButton = GameObject.FindGameObjectWithTag("Modal_Negative").GetComponentInChildren<Button>();
                _negative = negativeButton.GetComponentInChildren<Text>();
                positiveButton = GameObject.FindGameObjectWithTag("Modal_Positive").GetComponentInChildren<Button>();
                _positive = positiveButton.GetComponentInChildren<Text>();

                // add onclick listeners
                negativeButton.onClick.AddListener(Hide);
                negativeButton.onClick.AddListener(_click.Play);
                positiveButton.onClick.AddListener(Hide);
                positiveButton.onClick.AddListener(_click.Play);
            }
        }

        public void Show(bool oneButton, string heading, string message, string action = null, string negative = null, string positive = null)
        {
            // create the modal
            Initialise(oneButton);

            // set the heading, message, and button/s text
            _heading.text = heading;
            _message.text = message;

            if (oneButton)
                _action.text = action;
            else
            {
                _negative.text = negative;
                _positive.text = positive;
            }

            // activate the modal 
            _modal.SetActive(true);

        }

        private void Hide()
        {
            // deactivate the modal
            _modal.SetActive(false);

            // destory the prefab
            if (!_modal.activeSelf)
                Destroy(_modal);
        }
    }
}