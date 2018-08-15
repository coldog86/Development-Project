using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.EventSystems;

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
        public Text subHeading;

        [Header("Button")]
        public GameObject toogleLoginPanelButton;
        public GameObject toggleRegisterPanelButton;
        public GameObject skipButton;
        public GameObject googleButton;
        public GameObject facebookButton;

        [Header("Login")]
        public InputField existingUsernameInput;
        public InputField existingPasswordInput;
        public GameObject loginButton;

        [Header("Register")]
        public InputField newUsernameInput;
        public InputField newEmailInput;
        public InputField newPasswordInput;
        public InputField confirmPasswordInput;
        public GameObject registerButton;

        [Header("Connection")]
        [SerializeField]
        private const float _connectionTimeLimit = 10000.0f;

        [SerializeField]
        private float _connectionTimer = 0.0f;

        [Header("Player")]
        private Player _player;
        private string _playerString = "";

        [Header("Components")]
        private PlayerController _playerController;
        private SettingsController _settingsController;

        public GameObject dialogLoggedIn;
        public GameObject dialogLoggedOut;
        public GameObject dialogUsername;
		public GameObject dialogEmail;
		public GameObject dialogUIDPassword;
        //public GameObject dialogProfilePic;



        #endregion variables

        #region methods

        #region unity

        private void Awake()
        {
            FaceBookController.Instance.InitFB();
            DealWithFBMenus(FB.IsLoggedIn);

            subHeading.text = "";

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

            googleButton = GameObject.Find("Google");
            EventSystem.current.firstSelectedGameObject = googleButton;

            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();

            PlayGamesPlatform.Instance.Authenticate(LoginCallBack, true);
        }

        #endregion unity

        #region register specific

        public void Register()
        {
            FeedbackClick.Play();

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

            if (!string.IsNullOrEmpty(confirmPassword) && !string.IsNullOrEmpty(password) && (confirmPassword != password))
                FeedbackAlert.Show("Passwords don't match. Please try again");

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword) && confirmPassword == password)
            {
                if (ValidRegister(username, email, password))
                {
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

            _connectionTimer += Time.deltaTime;

            while (!registerRequest.isDone)
            {
                FeedbackAlert.Show("Attempting to create your account.");

                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("[LoginController] ValidRegister() : " + registerRequest.error);
                    return false;
                }
                else if (registerRequest.error != null)
                {
                    FeedbackAlert.Show("Connection error. Please try again.");
                    Debug.Log("[LoginController] ValidRegister() : " + registerRequest.error);
                    return false;
                }
                // extra check just to ensure a stream error doesn't come up
                else if (_connectionTimer > _connectionTimeLimit && registerRequest.error != null)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("[LoginController] ValidRegister() : " + registerRequest.error);
                    return false;
                }
            }

            if (registerRequest.isDone && registerRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.Log("[LoginController] ValidRegister() : " + registerRequest.error);
                return false;
            }

            if (registerRequest.isDone)
            {
                // check that the register request returned something
                if (!string.IsNullOrEmpty(registerRequest.text))
                {
                    _playerString = registerRequest.text;
                    Debug.Log(_playerString);

                    // if the retrieved register text doesn't have "ID" load login scene
                    if (!_playerString.Contains("ID"))
                    {
                        FeedbackAlert.Show("Registration failed. Please try again.");
                        return false;
                    }
                    // otherwise save the player information to PlayerPrefs and load menu scene
                    else
                    {
                        _player = PlayerJsonHelper.LoadPlayerFromServer(_playerString);

                        if (_player != null)
                            _playerController.Save(_player.ID, _player.username, _player.email, _player.password, _player.DOB, _player.questionsSubmitted,
                                _player.numQuestionsSubmitted, _player.numGamesPlayed, _player.totalPointsScore,
                                _player.TotalCorrectAnswers, _player.totalQuestionsAnswered);

                        FeedbackAlert.Show("Welcome, " + username + "!");
                        return true;
                    }

                }
            }
            return false;
        }

        #endregion register specific

        #region skip

        public void Skip()
        {
            FeedbackClick.Play();

            if (registerPanel.activeInHierarchy)
                FeedbackTwoButtonModal.Show("Warning!", "Registering as a guest limits what you can do.", "Register", "Cancel", ContinueAsGuest, FeedbackTwoButtonModal.Hide);
            else if (loginPanel.activeInHierarchy)
                FeedbackTwoButtonModal.Show("Warning!", "Logging in as a guest limits what you can do.", "Login", "Cancel", ContinueAsGuest, FeedbackTwoButtonModal.Hide);
        }

        private void ContinueAsGuest()
        {
            var guest = "Guest" + Random.Range(0, 1000000);

            if (ValidRegister(guest, guest, guest))
            {
                _playerController.SetPlayerType(PlayerStatus.Guest);
                LoadMenu();
            }
        }

        #endregion skip

        #region login specific

        public void Login()
        {
            FeedbackClick.Play();

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

            _connectionTimer += Time.deltaTime;

            while (!loginRequest.isDone)
            {
                FeedbackAlert.Show("Validating to credentials...");

                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("[LoginController] ValidLogin() : " + loginRequest.text);
                    return false;
                }
                else if (loginRequest.error != null)
                {
                    FeedbackAlert.Show("Connection error. Please try again.");
                    Debug.Log("[LoginController] ValidLogin() : " + loginRequest.text);
                    return false;
                }
                // extra check just to ensure a stream error doesn't come up
                else if (_connectionTimer > _connectionTimeLimit && loginRequest.error != null)
                {
                    FeedbackAlert.Show("Server error.");
                    Debug.LogError("[LoginController] ValidLogin() : " + loginRequest.text);
                    return false;
                }
            }

            if (loginRequest.isDone && loginRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.LogError("[LoginController] Login() : Server error " + loginRequest.error);
                return false;
            }

            if (loginRequest.isDone)
            {
                // check that the login request returned something
                if (!String.IsNullOrEmpty(loginRequest.text))
                {
                    _playerString = loginRequest.text;
                    Debug.Log("[LoginController] ValidLogin() : " + _playerString);

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
                                _player.numQuestionsSubmitted, _player.numGamesPlayed, _player.totalPointsScore,
                                _player.TotalCorrectAnswers, _player.totalQuestionsAnswered);

                        FeedbackAlert.Show("Welcome back, " + username + "!");
                        return true;
                    }

                }
            }
            return false;
        }

        #endregion login specific

        #region social media specific

        // TASK : to be completed when social media is integrated
        public void FacebookLogin()
        {
            FeedbackClick.Play();
            List<string> permissions = new List<string>();
            permissions.Add("public_profile");
			permissions.Add("email");
            FB.LogInWithReadPermissions(permissions, AuthCallBack);
        }

        void AuthCallBack(IResult result)
        {
            if (result.Error != null)
            {
				Debug.Log(result.Error);
            }
            else
            {
                if (FB.IsLoggedIn)
                {
                    FaceBookController.Instance.isLoggedIn = true;
                    FaceBookController.Instance.GetProfile();
                    Debug.Log("FB is logged in");
                }
                else
                {
                    Debug.Log("FB is not logged in");
                }

                DealWithFBMenus(FB.IsLoggedIn);
            }
			    
		}
        
		void DealWithFBMenus(bool isLoggedIn)
        {
            if (isLoggedIn)
            {
                dialogLoggedIn.SetActive(true);
                dialogLoggedOut.SetActive(false);
                entryPanel.SetActive(false);
                loginPanel.SetActive(false);
                registerPanel.SetActive(false);
                buttonPanel.SetActive(false);
                subHeading.text = "";
                googleButton.SetActive(false);
                facebookButton.SetActive(false);

				string username = FaceBookController.Instance.profileName;
				string email = FaceBookController.Instance.profileEmail;
				string password = FaceBookController.Instance.profileEmail;


				if (FaceBookController.Instance.profileName != null) {
					Text userName = dialogUsername.GetComponent<Text> ();
					userName.text = FaceBookController.Instance.profileName;
																							
				} else {
					StartCoroutine ("waitForProfileName");
				}

				if (FaceBookController.Instance.profileEmail != null) {
					Text userEmail = dialogUsername.GetComponent<Text> ();
					userEmail.text = FaceBookController.Instance.profileEmail;

				} else {
					StartCoroutine ("waitForProfileEmail");
				}

				if (FaceBookController.Instance.profileId != null) {
					Text userId = dialogUsername.GetComponent<Text> ();
					userId.text = FaceBookController.Instance.profileId;

				} else {
					StartCoroutine ("waitForProfileId");
				}

				//if (FaceBookController.Instance.profilePic != null) {
				//Image profilePic = DialogProfilePic.GetComponent<Image> ();
				//profilePic.sprite = FaceBookController.Instance.profilePic;
				//} else {

				//StartCoroutine ("waitForProfilePic");
			//}
				if (addFacebookUsers(username, email, password))
				{
					_playerController.SetPlayerType(PlayerStatus.LoggedIn);
					LoadMenu();
				}

			}
			else
			{
				dialogLoggedIn.SetActive(false);
				dialogLoggedOut.SetActive(true);
			}
		}

		IEnumerator waitForProfileName()
		{
			while (FaceBookController.Instance.profileName == null)
				yield return null;
			DealWithFBMenus(FB.IsLoggedIn);
		}

		IEnumerator waitForProfileEmail()
		{
			while (FaceBookController.Instance.profileEmail == null)
				yield return null;
			DealWithFBMenus(FB.IsLoggedIn);
		}

		IEnumerator waitForProfileId()
		{
			while (FaceBookController.Instance.profileId == null)
				yield return null;
			DealWithFBMenus(FB.IsLoggedIn);
		}

        //IEnumerator waitForProfilePic()
        //{
        //while (FaceBookController.Instance.profilePic == null) {
        //yield return null;
        //}
        //DealWithFBMenus (FB.IsLoggedIn);
        //}

		private bool addFacebookUsers(string username, string email, string password){

			username = FaceBookController.Instance.profileName;
			email = FaceBookController.Instance.profileEmail;
			password = FaceBookController.Instance.profileId;


			WWWForm form = new WWWForm();

			form.AddField("usernamePost", username);
			form.AddField("emailPost", email);
			form.AddField("passwordPost", password);

			WWW registerRequest = new WWW(ServerHelper.Host + ServerHelper.Register, form);

			_connectionTimer += Time.deltaTime;

			while (!registerRequest.isDone)
			{
				FeedbackAlert.Show("Adding details");

				if (_connectionTimer > _connectionTimeLimit)
				{
					FeedbackAlert.Show("Server time out.");
					Debug.LogError("[LoginController]  addFacebookUsers() : " + registerRequest.error);
					return false;
				}
				else if (registerRequest.error != null)
				{
					FeedbackAlert.Show("Connection error. Please try again.");
					Debug.Log("[LoginController] addFacebookUsers() : " + registerRequest.error);
					return false;
				}
				// extra check just to ensure a stream error doesn't come up
				else if (_connectionTimer > _connectionTimeLimit && registerRequest.error != null)
				{
					FeedbackAlert.Show("Server time out.");
					Debug.LogError("[LoginController] addFacebookUsers() : " + registerRequest.error);
					return false;
				}
			}

			if (registerRequest.isDone && registerRequest.error != null)
			{
				FeedbackAlert.Show("Connection error. Please try again.");
				Debug.Log("[LoginController]addFacebookUsers() : " + registerRequest.error);
				return false;
			}

			if (registerRequest.isDone)
			{
				// check that the register request returned something
				if (!string.IsNullOrEmpty(registerRequest.text))
				{
					_playerString = registerRequest.text;
					Debug.Log(_playerString);

					// if the retrieved register text doesn't have "ID" load login scene
					if (!_playerString.Contains("ID"))
					{
						FeedbackAlert.Show("Adding account failed. Please try again.");
						return false;
					}
					// otherwise save the player information to PlayerPrefs and load menu scene
					else
					{
						_player = PlayerJsonHelper.LoadPlayerFromServer(_playerString);

						if (_player != null)
							_playerController.Save(_player.ID, _player.username, _player.email, _player.password, _player.DOB, _player.questionsSubmitted,
								_player.numQuestionsSubmitted, _player.numGamesPlayed, _player.totalPointsScore,
								_player.TotalCorrectAnswers, _player.totalQuestionsAnswered);

						FeedbackAlert.Show("Welcome, " + username + "!");

						return true;

					}

				}
			}
			return false;
		}


	
		
        // TASK : to be completed when social media is integrated
        public void GoogleLogin()
        {
            FeedbackClick.Play();
            
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.Authenticate(LoginCallBack, false);

                FeedbackAlert.Show("Welcome, " + Social.localUser.userName + "!");
            }
            else
            {
                PlayGamesPlatform.Instance.SignOut();
                FeedbackAlert.Show("Login was not successful ");
            }
        }

        public void LoginCallBack(bool isLoggedIn)
        {
            if (isLoggedIn)
                Debug.Log("User logged in");
            else
                Debug.Log("User logged in failed ");
        }



        #endregion social media specific

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

            subHeading.text = "Login to Continue";
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

            subHeading.text = "Register to Continue";
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion navigation specific

        #endregion methods
    }
}