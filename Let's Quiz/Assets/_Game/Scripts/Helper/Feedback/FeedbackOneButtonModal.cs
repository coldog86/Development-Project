using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace _LetsQuiz
{
    public class FeedbackOneButtonModal : MonoBehaviour
    {
        #region variables

        [Header("Component")]
        public GameObject prefab;
        public static FeedbackOneButtonModal instance;
        private GameObject _modal;

        private static Text _heading;
        private static Text _message;
        private static Button _actionButton;
        private static Text _actionText;

        #endregion


        #region methods

        // creates the modal instance
        private void Awake()
        {
            // create instance of modal prefab as gameobject
            _modal = (GameObject)Instantiate(prefab);

            // ensure everything sticks around like a bad smell
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_modal);

            // find all the required components
            _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
            _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
            _actionButton = GameObject.FindGameObjectWithTag("Modal_Action").GetComponentInChildren<Button>();
            _actionText = _actionButton.GetComponentInChildren<Text>();

            // set instance
            instance = this;

            // deactivate modal
            instance._modal.SetActive(false);
        }

        // used to show the modal from external sources
        // closeOnAction is optional - might wish to not close it on action
        public static void Show(string heading, string message, string action, UnityAction eventToAction, bool closeOnAction = true)
        {
            // set the heading, message, and action text 
            _heading.text = heading;
            _message.text = message;
            _actionText.text = action;

            // set the action of the modal
            _actionButton.onClick.AddListener(eventToAction);

            // set if modal will close on action
            if (closeOnAction)
                _actionButton.onClick.AddListener(Hide);

            // after everything has been set, show the modal
            instance._modal.SetActive(true);
        }

        // used to hide the modal from external sources
        public static void Hide()
        {
            // hide the modal
            instance._modal.SetActive(false);
        }

        #endregion
    }
}

