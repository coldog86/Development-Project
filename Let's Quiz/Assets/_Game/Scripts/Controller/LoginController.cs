using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using _Game.Scripts.Helper.Feedback;

namespace _Game.Scripts.Controller
{
    public class LoginController : MonoBehaviour
    {
        private const string TAG = "LoginController";

        [Header("Component")]
        public InputField usernameInput;
        public InputField passwordInput;

        [Header("Feedback")]
        [SerializeField]
        private FeedbackAlert _alert;
        [SerializeField]
        private FeedbackModal _modal;

        [Header("Setting")]
        public int menuIndex = 2;

        [Header("Debug")]
        public string testUsername = "test@email.com";
        public string testPassword = "123456";

        private void Start()
        {
            if (!usernameInput)
                Debug.LogError(TAG + " Username Input field is null");
            if (!passwordInput)
                Debug.LogError(TAG + " Password Input field is null");

            if (!_alert)
                _alert = GetComponent<FeedbackAlert>();

            if (!_modal)
                _modal = GetComponent<FeedbackModal>();
        }

        public void EmailLogin()
        {
            var username = usernameInput.text;
            var password = passwordInput.text;

            if (string.IsNullOrEmpty(username))
                _alert.Show("Username cannot be empty.", 2.5f);
            else if (string.IsNullOrEmpty(password))
                _alert.Show("Password cannot be empty.", 2.5f);
            else if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (ValidateLogin(username, password))
                    LoadMenu();
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            if (username == testUsername && password == testPassword)
            {
                _alert.Show("Logging in...", 1.0f);
                return true;
            }
            else if (username != testUsername)
            {
                _alert.Show("Username provided is incorrect.", 2.5f);
                return false;
            }
            else if (password != testPassword)
            {
                _alert.Show("Password provided is incorrect.", 2.5f);
                return false;
            }
            return false;
        }

        // Placeholder for Michelle
        public void FacebookLogin()
        {
            _alert.Show("Not implemented yet.", 2.5f);
        }

        // Placeholder for Michelle
        public void GoogleLogin()
        {
            _alert.Show("Not implemented yet.", 2.5f);
        }

        public void ForgotPassword()
        {
            _alert.Show("Forgot Password", 1.0f);
        }

        private void LoadMenu()
        {
            SceneManager.LoadScene(menuIndex, LoadSceneMode.Single);
        }
    }
}

