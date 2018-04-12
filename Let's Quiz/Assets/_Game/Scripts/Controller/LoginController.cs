using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class LoginController : MonoBehaviour
    {
        private const string TAG = "LoginController";

        [Header("Component")]
        public InputField usernameInput;
        public InputField passwordInput;

        [Header("Setting")]
        public int menuIndex = 2;

        [Header("Debug")]
        public string testUsername = "test@email.com";
        public string testPassword = "123456";

        [SerializeField]
        private FeedbackAlert _alert;
        [SerializeField]
        private FeedbackClick _click;
        [SerializeField]
        private FeedbackModal _modal;

        private void Start()
        {
            if (!usernameInput)
                Debug.LogError(TAG + " Username Input field is null");
            if (!passwordInput)
                Debug.LogError(TAG + " Password Input field is null");

            _alert = FindObjectOfType<FeedbackAlert>();
            _click = FindObjectOfType<FeedbackClick>();
            _modal = FindObjectOfType<FeedbackModal>();
        }

        // TASK : PLACEHOLDER COL
        public void SkipLogin()
        {
            _click.Play();
            _modal.Show(false, "Warning!", "Logging in as a guest limits what you can do.", null, "Cancel", "Login");
            _modal.positiveButton.onClick.AddListener(LoadMenu);
        }

        // TASK : PLACEHOLDER COL
        public void EmailLogin()
        {
            _click.Play();

            var username = usernameInput.text;
            var password = passwordInput.text;

            if (string.IsNullOrEmpty(username))
                _alert.Show("Username cannot be empty.");
            else if (string.IsNullOrEmpty(password))
                _alert.Show("Password cannot be empty.");
            else if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (ValidateLogin(username, password))
                    LoadMenu();
            }
        }

        // TASK : PLACEHOLDER COL
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

        // TASK : PLACEHOLDER FOR MICHELLE
        public void FacebookLogin()
        {
            _click.Play();
            _alert.Show("Not implemented yet...");
        }

        // TASK : PLACEHOLDER FOR MICHELLE
        public void GoogleLogin()
        {
            _click.Play();
            _alert.Show("Not implemented yet...");
        }

        // NOTE : UNSURE IF REQUIRED, PUT THERE TO COVER ALL BASES
        public void ForgotPassword()
        {
            _click.Play();
            _alert.Show("Forgot Password...");
        }

        private void LoadMenu()
        {
            SceneManager.LoadScene(menuIndex, LoadSceneMode.Single);
        }
    }
}

