using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class LoginController : MonoBehaviour
    {
        #region variables

        [Header("Component")]
        public InputField usernameInput;
        public InputField passwordInput;

        [Header("Debug")]
        public string testUsername = "test@email.com";
        public string testPassword = "123456";

        private FeedbackClick _click;
        private SettingsController _settingsController;
        private LoadHelper _loadHelper;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            if (!usernameInput)
                Debug.LogError("Username Input field is null");
            if (!passwordInput)
                Debug.LogError("Password Input field is null");

            _settingsController = FindObjectOfType<SettingsController>();
            _click = FindObjectOfType<FeedbackClick>();
            _loadHelper = FindObjectOfType<LoadHelper>();
            Destroy(_loadHelper);
        }

        #endregion

        // TASK : PLACEHOLDER
        public void SkipLogin()
        {
            _click.Play();
            FeedbackTwoButtonModal.Show("Warning!", "Logging in as a guests limits what you can do.", "Login", "Cancel", LoadMenuAsGuest, FeedbackTwoButtonModal.Hide);
        }

        #region email specific

        // TASK : PLACEHOLDER FOR CHARNES
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

        // TASK : PLACEHOLDER FOR CHARNES
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

        #endregion

        #region social media specific

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

        #endregion

        #region navigation specific

        public void LoadMenu()
        {
            _settingsController.SetPlayerType(PlayerStatus.Existing);
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        public void LoadMenuAsGuest()
        {
            _settingsController.SetPlayerType(PlayerStatus.Guest);
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion

        #endregion
    }
}

