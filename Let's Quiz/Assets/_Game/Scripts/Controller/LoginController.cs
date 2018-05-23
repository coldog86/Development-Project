using System;
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

        [Header("Panel")]
        public GameObject entryPanel;
        public GameObject loginPanel;
        public GameObject registerPanel;
        public GameObject buttonPanel;

        [Header("Button")]
        public GameObject toogleLoginPanelButton;
        public GameObject toggleRegisterPanelButton;
        public GameObject skipButton;
        //public GameObject googleButton;
        //public GameObject facebookButton;

        [Header("Existing User")]
        public InputField existingUsernameInput;
        public InputField existingPasswordInput;
        public GameObject loginButton;
       
        [Header("New User")]
        public InputField newUsernameInput;
        public InputField newEmailInput;
        public InputField newPasswordInput;
        public InputField confirmPasswordInput;
        public GameObject registerButton;

        [Header("Debug")]
        public string testUsername = "test@email.com";
        public string testPassword = "123456";

        [Header("Connection")]
        [SerializeField]
        private float _connectionTimeLimit = 10000.0f;
        [SerializeField]
        private float _connectionTimer = 0.0f;

        [Header("Player")]
        private Player _player;
        private string _playerString = "";

        [Header("Components")]
        private FeedbackClick _click;
        private PlayerController _playerController;
        private SettingsController _settingsController;

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
            if (!registerButton)
                Debug.LogError("Create User button is null");
            if (!loginButton)
                Debug.LogError("Login button is null");
            if (!registerButton)
                Debug.LogError("Register User button is null");
            if (!skipButton)
                Debug.LogError("Skip button is null");

            entryPanel.SetActive(true);
            loginPanel.SetActive(false);
            registerPanel.SetActive(false);
            loginButton.SetActive(false);
            skipButton.SetActive(false);
            registerButton.SetActive(false);
            buttonPanel.SetActive(false);
            //googleButton.SetActive(false);
            //facebookButton.SetActive(false);
        }

        private void Start()
        {
            _settingsController = FindObjectOfType<SettingsController>();
            _settingsController.Load();

            _playerController = FindObjectOfType<PlayerController>();
            _playerController.Load();

            _click = FindObjectOfType<FeedbackClick>();
        }

        private void Update()
        {
            #if PLATFORM_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape))
                FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", Application.Quit, FeedbackTwoButtonModal.Hide);
            #endif
        }

        #endregion

        #region register specific

        public void Register()
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
                if (ValidRegister(username, email, password))
                {
                    _playerController.SetUsername(username);
                    _playerController.SetPassword(password);
                    _playerController.SetEmail(email);
                    _playerController.SetPlayerType(PlayerStatus.LoggedIn);
                    LoadMenu();
                }      
            }
        }

        private bool ValidRegister(string username, string email, string password)
        {
            WWWForm form = new WWWForm();

            form.AddField("usernamePost", username);
            form.AddField("emailPost", email);
            form.AddField("passwordPost", password);

            WWW registerRequest = new WWW(ServerHelper.Host + ServerHelper.Register, form);

            while (!registerRequest.isDone)
            { 
                _connectionTimer += Time.deltaTime;

                FeedbackAlert.Show("Attempting to create your account.");

                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("LoginController : ValidRegister() : " + registerRequest.error);
                    return false;
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || registerRequest.error != null)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("LoginController : ValidRegister() : " + registerRequest.error);
                    return false;
                }
            }

            if (registerRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.Log("LoginController : ValidRegister() : " + registerRequest.error);
                return false;
            }

            if (registerRequest.isDone)
            {
                // check that the login request returned something
                if (!String.IsNullOrEmpty(registerRequest.text))
                {
                    _playerString = registerRequest.text;
                    Debug.Log(_playerString);

                    // if the retrieved register text doesn't have "ID" load login scene
                    if (!_playerString.Contains("ID"))
                    {
                        FeedbackAlert.Show("User not found. Please try again.");
                        return false;
                    }
                    // otherwise save the player information to PlayerPrefs and load menu scene
                    else
                    {
                        _player = PlayerJsonHelper.LoadPlayerFromServer(_playerString);

                        if (_player != null)
                            _playerController.Save(_player.ID, _player.username, _player.email, _player.password, _player.DOB, _player.questionsSubmitted, 
                                _player.numQuestionsSubmitted, _player.numGamesPlayed, _player.highestScore, 
                                _player.numCorrectAnswers, _player.totalQuestionsAnswered);

                        FeedbackAlert.Show("Welcome " + username + "!");
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region login specific

        public void Skip()
        {
            _click.Play();

            _playerController.SetPlayerType(PlayerStatus.Guest);

            if (registerPanel.activeInHierarchy)
                FeedbackTwoButtonModal.Show("Warning!", "Registering in as a guest limits what you can do.", "Register", "Cancel", LoadMenu, FeedbackTwoButtonModal.Hide);
            else if (loginPanel.activeInHierarchy)
                FeedbackTwoButtonModal.Show("Warning!", "Logging in as a guest limits what you can do.", "Login", "Cancel", LoadMenu, FeedbackTwoButtonModal.Hide);
        }

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

            WWW loginRequest = new WWW(ServerHelper.Host + ServerHelper.Login, form);
          
            while (!loginRequest.isDone)
            {
                FeedbackAlert.Show("Validating to credentials...");
                _connectionTimer += Time.deltaTime;

                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("LoginController : ValidLogin() : " + loginRequest.error);
                    return false;
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || loginRequest.error != null)
                {
                    FeedbackAlert.Show("Server error.");
                    Debug.LogError("LoginController : ValidLogin() : " + loginRequest.error);
                    return false;
                }    
            }

            if (loginRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.Log("LoginController : ValidLogin() : " + loginRequest.error);
                return false;
            }

            if (loginRequest.isDone)
            {
                // check that the login request returned something
                if (!String.IsNullOrEmpty(loginRequest.text))
                {
                    _playerString = loginRequest.text;
                    Debug.Log(_playerString);

                    // if the retrieved login text doesn't have "ID" load login scene
                    if (!_playerString.Contains("ID"))
                    {
                        FeedbackAlert.Show("User not found. Please try again.");
                        return false;
                    }
                    // otherwise save the player information to PlayerPrefs and load menu scene
                    else
                    {
                        _player = PlayerJsonHelper.LoadPlayerFromServer(_playerString);

                        if (_player != null)
                            _playerController.Save(_player.ID, _player.username, _player.email, _player.password, _player.DOB, _player.questionsSubmitted, 
                                _player.numQuestionsSubmitted, _player.numGamesPlayed, _player.highestScore, 
                                _player.numCorrectAnswers, _player.totalQuestionsAnswered);

                        FeedbackAlert.Show("Welcome back " + username + "!");
                        return true;
                    }
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

        public void ToggleLoginPanel()
        {
            entryPanel.SetActive(false);
            loginPanel.SetActive(true);
            registerPanel.SetActive(false);
            buttonPanel.SetActive(true);
            loginButton.SetActive(true);
            skipButton.SetActive(true);
            registerButton.SetActive(false);
            //googleButton.SetActive(true);
            //facebookButton.SetActive(true);
            toogleLoginPanelButton.SetActive(false);
            toggleRegisterPanelButton.SetActive(false);
        }

        public void ToggleRegisterPanel()
        {
            entryPanel.SetActive(false);
            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
            buttonPanel.SetActive(true);
            loginButton.SetActive(false);
            skipButton.SetActive(true);
            registerButton.SetActive(true);
            //googleButton.SetActive(true);
            //facebookButton.SetActive(true);
            toogleLoginPanelButton.SetActive(false);
            toggleRegisterPanelButton.SetActive(false);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion

        #endregion
    }
}

