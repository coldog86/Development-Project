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

        private FeedbackClick _click;

        private void Start()
        {
            if (!usernameInput)
                Debug.LogError(TAG + " Username Input field is null");
            if (!passwordInput)
                Debug.LogError(TAG + " Password Input field is null");

            _click = FindObjectOfType<FeedbackClick>();
        }

        // TASK : PLACEHOLDER FOR COL
        public void SkipLogin()
        {
            _click.Play();
            FeedbackTwoButtonModal.Show("Warning!", "Logging in as a guests limits what you can do.", "Login", "Cancel", LoadMenu, FeedbackTwoButtonModal.Hide);
        }

        // TASK : PLACEHOLDER FOR COL
        public void EmailLogin()
        {
            _click.Play();

            var username = usernameInput.text;
            var password = passwordInput.text;

            if (string.IsNullOrEmpty(username))
                FeedbackAlert.Show("Username cannont be empty.");
            else if (string.IsNullOrEmpty(password))
                FeedbackAlert.Show("Password cannont be empty.");
            else if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (ValidateLogin(username, password))
                    LoadMenu();
            }
        }

        // TASK : PLACEHOLDER FOR COL
        private bool ValidateLogin(string username, string password)
        {
            if (username == testUsername && password == testPassword)
            {
                FeedbackAlert.Show("Logging in...", 1.0f);
                return true;
            }
            else if (username != testUsername)
            {
                FeedbackAlert.Show("Username provided is incorrect.", 2.5f);
                return false;
            }
            else if (password != testPassword)
            {
                FeedbackAlert.Show("Password provided is incorrect.", 2.5f);
                return false;
            }
            return false;
        }

        // TASK : PLACEHOLDER FOR MICHELLE
        public void FacebookLogin()
        {
            _click.Play();
            FeedbackAlert.Show("Not implemented yet...");
        }

        // TASK : PLACEHOLDER FOR MICHELLE
        public void GoogleLogin()
        {
            _click.Play();
            FeedbackAlert.Show("Not implemented yet...");
        }

        // NOTE : UNSURE IF REQUIRED, PUT THERE TO COVER ALL BASES
        public void ForgotPassword()
        {
            _click.Play();
            FeedbackAlert.Show("Forgot Password...");
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(menuIndex, LoadSceneMode.Single);
        }
    }
}

