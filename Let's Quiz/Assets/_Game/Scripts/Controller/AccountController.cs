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

            if (!username)
                Debug.Log("Username Input Field is null");
            if (!email)
                Debug.Log("Email Input Field is null");
        }

        private void Start()
        {
            username.text = _playerController.GetUsername();
            email.text = _playerController.GetEmail();
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