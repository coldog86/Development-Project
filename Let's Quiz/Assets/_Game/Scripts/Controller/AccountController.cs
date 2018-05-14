using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.Remoting.Messaging;


namespace _LetsQuiz
{
    public class AccountController : MonoBehaviour
    {
        #region variables

        private FeedbackClick _click;
        private PlayerController _playerController;
        private GameObject _warningPanel;
        private GameObject _userPanel;

        [Header("Components")]
        public Text username;
        public Text email;

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            _click = FindObjectOfType<FeedbackClick>();

            _playerController = FindObjectOfType<PlayerController>();
            _playerController.Load();

            _warningPanel = GameObject.FindGameObjectWithTag("Panel_Warning");
            _warningPanel.SetActive(false);

            _userPanel = GameObject.FindGameObjectWithTag("Panel_User");
            _userPanel.SetActive(false);

            if (!username)
                Debug.Log("Username Input Field is null");
            if (!email)
                Debug.Log("Email Input Field is null");
        }

        private void Start()
        {
            if (_playerController.GetPlayerType() != PlayerStatus.LoggedIn)
            {
                _warningPanel.SetActive(true);
                _userPanel.SetActive(false);
            }
            else
            {
                _warningPanel.SetActive(false);
                _userPanel.SetActive(true);
                username.text = _playerController.GetUsername();
                email.text = _playerController.GetEmail();
            }  
        }

        #endregion

        #region navigation specific

        public void BackToMenu()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion


        #endregion
    }
}