using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Helper.Feedback
{
    public class FeedbackModal : MonoBehaviour
    {
        [Header("Component")]
        public GameObject oneButtonPrefab;
        public GameObject twoButtonPrefab;

        private GameObject _modal;
        private Text _heading;
        private Text _message;
        private Button _actionButton;
        private Text _action;
        private Button _cancelButton;
        private Text _cancel;
        private Button _okayButton;
        private Text _okay;

        private void Initialise(bool oneButton)
        {
            if (oneButton)
            {
                _modal = (GameObject)Instantiate(oneButtonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
                _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
                _actionButton = GameObject.FindGameObjectWithTag("Modal_Action").GetComponentInChildren<Button>();
                _action = _actionButton.GetComponentInChildren<Text>();
                _actionButton.onClick.AddListener(Hide);
            }
            else
            {
                _modal = (GameObject)Instantiate(twoButtonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                _heading = GameObject.FindGameObjectWithTag("Modal_Heading").GetComponent<Text>();
                _message = GameObject.FindGameObjectWithTag("Modal_Message").GetComponent<Text>();
                _cancelButton = GameObject.FindGameObjectWithTag("Modal_Negative").GetComponentInChildren<Button>();
                _cancel = _cancelButton.GetComponentInChildren<Text>();
                _cancelButton.onClick.AddListener(Hide);
                _okayButton = GameObject.FindGameObjectWithTag("Modal_Positive").GetComponentInChildren<Button>();
                _okay = _okayButton.GetComponentInChildren<Text>();
                _okayButton.onClick.AddListener(Hide);
            }
        }

        public void Show(bool oneButton, string heading, string message, string action = null, string cancel = null, string okay = null)
        {
            Initialise(oneButton);

            _modal.SetActive(true);
            _heading.text = heading;
            _message.text = message;

            if (oneButton)
                _action.text = action;
            else
            {
                _cancel.text = cancel;
                _okay.text = okay;
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