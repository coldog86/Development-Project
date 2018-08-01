﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace _LetsQuiz
{
    public class FeedbackTwoButtonModal : MonoBehaviour
    {
        #region variables

        [Header("Component")]
        private static GameObject _instance;

        private static Text _heading;
        private static Text _message;
        private static Button _positiveButton;
        private static Text _positiveText;
        private static Button _negativeButton;
        private static Text _negativeText;

        #endregion

        #region methods

        // creates the modal instance
        private static void Create()
        {
            // create instance of modal prefab as gameobject
            _instance = Instantiate(Resources.Load<GameObject>("Feedback/ModalTwoButton"));

            // find all the required components
            _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
            _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
            _positiveButton = GameObject.FindGameObjectWithTag("Modal_Positive").GetComponentInChildren<Button>();
            _positiveText = _positiveButton.GetComponentInChildren<Text>();
            _negativeButton = GameObject.FindGameObjectWithTag("Modal_Negative").GetComponentInChildren<Button>();
            _negativeText = _negativeButton.GetComponentInChildren<Text>();

            // deactivate modal
            _instance.SetActive(false);
        }

        // used to show the modal from external sources
        // closeOnAction is optional - might wish to not close it on action
        public static void Show(string heading, string message, string postive, string negative, UnityAction positiveAction, UnityAction negativeAction, bool closeOnAction = true)
        {
            Create();

            // set the heading, message, and button text 
            _heading.text = heading;
            _message.text = message;
            _positiveText.text = postive;
            _negativeText.text = negative;

            // set the actions of the buttons
            _positiveButton.onClick.AddListener(positiveAction);
            _negativeButton.onClick.AddListener(negativeAction);

            // set if modal will close on action
            if (closeOnAction)
            {
                _positiveButton.onClick.AddListener(Hide);
                _negativeButton.onClick.AddListener(Hide);
            }

            // after everything has been set, show the modal
            _instance.SetActive(true);
        }

        // used to hide the modal from external sources
        public static void Hide()
        {
            // hide the modal
            _instance.SetActive(false);
            if (!_instance.activeInHierarchy)
                Destroy(_instance);
        }

        #endregion
    }
}
