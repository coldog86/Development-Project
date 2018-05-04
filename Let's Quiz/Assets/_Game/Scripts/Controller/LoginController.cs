using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace _LetsQuiz
{
    public class LoginController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        public GameObject existingUserPanel;
        public GameObject newUserPanel;
        public GameObject skipButton;

        [Header("Existing User")]
        public InputField existingUsernameInput;
        public InputField existingPasswordInput;
        public GameObject loginButton;
       
        [Header("New User")]
        public InputField newUsernameInput;
        public InputField newEmailInput;
        public InputField newPasswordInput;
        public InputField confirmPasswordInput;
        public GameObject registerUserButton;

        [Header("Debug")]
        public string testUsername = "test@email.com";
        public string testPassword = "123456";

        [Header("Server")]
        [SerializeField]
        private string _hostUrl = "www.41melquizgame.xyz/LQ/";
        [SerializeField]
        private string _loginUserFile = "loginExistingUserWithEmail.php";
        [SerializeField]
        private string _addUserFile = "addUser.php";

        [Header("Connection")]
        [SerializeField]
        private float _connectionTimeLimit = 10000.0f;
        [SerializeField]
        private float _connectionTimer = 0.0f;

        private FeedbackClick _click;
        private PlayerController _playerController;
        private SettingsController _settingsController;
        private LoadHelper _loadHelper;

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            // NOTE : DEBUG PURPOSES ONLY - ENSURES ALL REFERENCES ARE AVAILABLE
            if (!existingUsernameInput)
                Debug.LogError("Existing username input field is null");
            if (!existingPasswordInput)
                Debug.LogError("Existing password input field is null");
            if (!newUsernameInput)
                Debug.LogError("New username input field is null");
            if (!newEmailInput)
                Debug.LogError("New email input field is null");
            if (!newPasswordInput)
                Debug.LogError("New password input field is null");
            if (!confirmPasswordInput)
                Debug.LogError("Confirm password input field is null");
            if (!loginButton)
                Debug.LogError("Login button is null");
            if (!registerUserButton)
                Debug.LogError("Create User button is null");
            if (!loginButton)
                Debug.LogError("Login button is null");
            if (!registerUserButton)
                Debug.LogError("Register User button is null");
            if (!skipButton)
                Debug.LogError("Skip button is null");

            existingUserPanel.SetActive(false);
            newUserPanel.SetActive(false);
            loginButton.SetActive(false);
            skipButton.SetActive(false);
            registerUserButton.SetActive(false);
        }

        private void Start()
        {
            _settingsController = FindObjectOfType<SettingsController>();
            _settingsController.LoadPlayerSettings();

            _playerController = FindObjectOfType<PlayerController>();
            _playerController.LoadPlayer();

            if (_playerController.GetPlayerType() == PlayerStatus.LoggedOut)
                ExistingMember();
            else
                RegisterMember();

            _click = FindObjectOfType<FeedbackClick>();
            _loadHelper = FindObjectOfType<LoadHelper>();
            Destroy(_loadHelper);
        }

        private void Update()
        {
            #if PLATFORM_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape))
                FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", QuitGame, FeedbackTwoButtonModal.Hide);
            #endif
        }

        #endregion

        #region create specific

        public void Create()
        {
            _click.Play();

            // Get text from inputs
            var username = newUsernameInput.text;
            var email = newEmailInput.text;
            var password = newPasswordInput.text;
            var confirmPassword = confirmPasswordInput.text;

            if (string.IsNullOrEmpty(username))
                FeedbackAlert.Show("Username cannont be empty.");
            
            if (string.IsNullOrEmpty(email))
                FeedbackAlert.Show("Email cannont be empty.");
            
            if (string.IsNullOrEmpty(password))
                FeedbackAlert.Show("Password cannont be empty.");
            
            if (string.IsNullOrEmpty(confirmPassword))
                FeedbackAlert.Show("Confirm password cannont be empty.");
            
            if (confirmPassword != password)
                FeedbackAlert.Show("Passwords don't match. Please try again");
            
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
            {
                if (ValidCreate(username, email, password))
                {
                    _playerController.SetUsername(username);
                    _playerController.SetPassword(password);
                    _playerController.SetPlayerType(PlayerStatus.LoggedIn);
                    LoadMenu();
                }      
            }
        }

        private bool ValidCreate(string username, string email, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("usernamePost", username);
            form.AddField("emailPost", email);
            form.AddField("passwordPost", password);
            WWW createRequest = new WWW(_hostUrl + _addUserFile, form);

            while (!createRequest.isDone)
            { 
                _connectionTimer += Time.deltaTime;
                FeedbackAlert.Show("Attempting to create your account.");

                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Time out error. Please try again.");
                    Debug.LogError(createRequest.error);
                    Debug.Log(createRequest.text);
                    return false;
                }

            }

            if (createRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.Log(createRequest.error);
                return false;
            }

            if (createRequest.isDone)
            {
                FeedbackAlert.Show("Welcome " + username);
                Debug.Log("Welcome " + username);
                return true;
            }

            return false;
        }

        #endregion

        #region login specific

        // NOTE : ELABORATION 1
        // TASK : PLACEHOLDER FOR CHARNES
        public void SkipLogin()
        {
            _click.Play();
            _playerController.SetPlayerType(PlayerStatus.Guest);
            FeedbackTwoButtonModal.Show("Warning!", "Logging in as a guest limits what you can do.", "Login", "Cancel", LoadMenu, FeedbackTwoButtonModal.Hide);
        }

        // NOTE : ELABORATION 1
        // TASK : PLACEHOLDER FOR CHARNES
        public void Login()
        {
            _click.Play();

            string username = existingUsernameInput.text;
            string password = existingPasswordInput.text;


            if (string.IsNullOrEmpty(username))
                FeedbackAlert.Show("Username cannont be empty.");

            if (string.IsNullOrEmpty(password))
                FeedbackAlert.Show("Password cannont be empty.");

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (ValidLogin(username, password))
                {
                    _playerController.SetUsername(username);
                    _playerController.SetPassword(password);
                    _playerController.SetPlayerType(PlayerStatus.LoggedIn);
                    LoadMenu();
                }
            }
        }

        private bool ValidLogin(string username, string password)
        {
            WWWForm form = new WWWForm();

            form.AddField("usernamePost", username);
            form.AddField("passwordPost", password);

            WWW loginRequest = new WWW(_hostUrl + _loginUserFile, form);
          
            while (!loginRequest.isDone)
            {
                FeedbackAlert.Show("Validating to credentials...");
                _connectionTimer += Time.deltaTime;

                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("Server time out.");
                    Debug.LogError(loginRequest.error);
                    return false;
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || loginRequest.error != null)
                {
                    FeedbackAlert.Show("Server error.");
                    Debug.LogError(loginRequest.error);
                    return false;
                }    
            }

            if (loginRequest.isDone)
            {
                if (!loginRequest.text.Contains("ID"))
                {
                    FeedbackAlert.Show("User not found. Please try again.");
                    return false;
                }
                else
                {
                    FeedbackAlert.Show("Welcome back " + username);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region social media specific

        // TASK : PLACEHOLDER
        public void FacebookLogin()
        {
            _click.Play();
            FeedbackAlert.Show("Not implemented yet.");
        }

        // TASK : PLACEHOLDER
        public void GoogleLogin()
        {
            _click.Play();
            FeedbackAlert.Show("Not implemented yet.");
        }

        #endregion

        #region navigation specific

        private void ExistingMember()
        {
            existingUserPanel.SetActive(true);
            newUserPanel.SetActive(false);
            loginButton.SetActive(true);
            skipButton.SetActive(true);
            registerUserButton.SetActive(false);
        }

        private void RegisterMember()
        {
            existingUserPanel.SetActive(false);
            newUserPanel.SetActive(true);
            loginButton.SetActive(false);
            skipButton.SetActive(true);
            registerUserButton.SetActive(true);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        private void QuitGame()
        {
            Application.Quit();

            // NOTE : DEBUG PURPOSES ONLY
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        #endregion

        #region obsolete - only keeping for historical purposes

        [Obsolete("ValidateLogin is deprecated, please use ValidLogin instead.", true)]
        private bool ValidateLogin(string username, string password)
        {
            if (username == testUsername && password == testPassword)
            {
                FeedbackAlert.Show("Logging in.", 1.0f);
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

        #endregion
    }
}

