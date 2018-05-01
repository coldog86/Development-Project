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
		public InputField createUsernameInput;
		public InputField createPassword1Input;
		public InputField createPassword2Input;
		public InputField createEmailInput;
		public GameObject newLoginPanel;
		public GameObject returningUserLoginPanel;
		public GameObject buttonPanel;

		public InputField passwordInput;
        public InputField usernameInput;


		[Header("Debug")]
		public string testUsername = "test@email.com";
		public string testPassword = "123456";

		private FeedbackClick _click;
		private SettingsController _settingsController;
		private LoadHelper _loadHelper;
		private string _password;


		#endregion

		#region methods

		#region unity

		private void Start()
		{
			if(PlayerPrefs.HasKey("login"))
			{
				Debug.Log("Welcome back " + PlayerPrefs.GetString("username"));
			}
			else
			{
				
			}

		}

		public void createUser()
		{
			returningUserLoginPanel.SetActive(false);
			newLoginPanel.SetActive(true);

			Button[] buttonArray = buttonPanel.GetComponentsInChildren<Button>(true);

			Button createAccount = buttonArray[1];
			createAccount.gameObject.SetActive(false);
			Button createUserButton = buttonArray[2];
			createUserButton.gameObject.SetActive(true);

		}


		public void checkInputFields ()
		{
			//TODO each of these need some que to the user, if they flashed or changed color that would do...
			if(createPassword2Input.gameObject.activeInHierarchy)
			{
				if(_password != createPassword2Input.text)
				{
					Debug.Log("passwords do not match");
				}
				if(_password == createPassword2Input.text)
				{
					updateDataBase();
				}
			}

			if (createPassword1Input.gameObject.activeInHierarchy) 
			{
				if (createUsernameInput.text == "") 
				{
					Debug.Log ("enter username");
				}
				if (createEmailInput.text == "") 
				{
					Debug.Log ("enter email");
				}
				if (createPassword1Input.text == "") 
				{
					Debug.Log ("enter password");
				} else
				{
					_password = createPassword1Input.text;
					createPassword1Input.gameObject.SetActive (false);
					createEmailInput.gameObject.SetActive (false);
					createUsernameInput.gameObject.SetActive (false);
					createPassword2Input.gameObject.SetActive (true);
				}
			}



		}


		private void updateDataBase()
		{
			WWWForm form = new WWWForm ();
			form.AddField ("usernamePost", createUsernameInput.text);
			form.AddField ("emailPost", createEmailInput.text);
			form.AddField ("passwordPost", createPassword2Input.text);
			WWW www = new WWW ("WWW.41melquizgame.xyz/LQ/addUser.php", form); 

			float _timer = 5;
			while (!www.isDone) { //starts a timer to stop trying to establish connection to server
				if (_timer < 0) {
					Debug.LogError ("time out error on server");
					Debug.LogError (www.error);	
					Debug.Log (www.text);
					break;
				}
				//_timer -= Time.deltaTime; //TODO fix this, the timer was instantly running out for some reason

			}
			if (www.isDone) 
			{
				Debug.Log(www.text);
				Debug.Log ("user added! " + createUsernameInput.text);
			}

		}


		#endregion

		#region email specific

		// TASK : PLACEHOLDER FOR CHARNES
		public void SkipLogin()
		{
			_click.Play();
		//	FeedbackTwoButtonModal.Show("Warning!", "Logging in as a guest limits what you can do.", "Login", "Cancel", LoadMenuAsGuest, FeedbackTwoButtonModal.Hide);
		}

		// TASK : PLACEHOLDER FOR CHARNES
		public void EmailLogin()
		{

			if (!usernameInput)
				Debug.LogError("Username Input field is null");
				//TODO there needs to be an onscreen que as users cannot see the error log and may not know why nothing is happening
			if (!passwordInput)
				Debug.LogError("Password Input field is null");
            //TODO there needs to be an onscreen que as users cannot see the error log and may not know why nothing is happening

            if (usernameInput && passwordInput)
                loginExistingEmailAccount();

			var username = usernameInput.text;
			var password = passwordInput.text;

            if (string.IsNullOrEmpty(username))
            {
                //FeedbackAlert.Show("Username cannont be empty.");
                Debug.Log("Username cannont be empty.");
            }
            else if (string.IsNullOrEmpty(password))
            {
                FeedbackAlert.Show("Password cannont be empty.");
                Debug.Log("Password cannont be empty.");
            }
            else if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (ValidateLogin(username, password))
                    LoadMenu();
            }
		}

        private void loginExistingEmailAccount()
        {
            WWWForm form = new WWWForm();
            form.AddField("usernamePost", usernameInput.text);
            form.AddField("passwordPost", passwordInput.text);
            WWW www = new WWW("WWW.41melquizgame.xyz/LQ/loginExistingUserWithEmail.php", form);

            float _timer = 5;
            while (!www.isDone)
            { //starts a timer to stop trying to establish connection to server
                if (_timer < 0)
                {
                    Debug.LogError("time out error on server");
                    Debug.LogError(www.error);
                    Debug.Log(www.text);
                    break;
                }
                //_timer -= Time.deltaTime; //TODO fix this, the timer was instantly running out for some reason

            }
            if (www.isDone)
            {
                Debug.Log("welcome back " + usernameInput.text);
                Debug.Log(www.text);
            }

        }

    

    // TASK : PLACEHOLDER FOR CHARNES
    private bool ValidateLogin(string username, string password)
		{
			if (username == testUsername && password == testPassword)
			{
				//FeedbackAlert.Show("Logging in...", 1.0f);
				return true;
			}
			else if (username != testUsername)
			{
				//FeedbackAlert.Show("Username provided is incorrect.", 2.5f);
				return false;
			}
			else if (password != testPassword)
			{
				//FeedbackAlert.Show("Password provided is incorrect.", 2.5f);
				return false;
			}
			return false;
		}

		#endregion

		#region social media specific

		// TASK : PLACEHOLDER
		public void FacebookLogin()
		{
			_click.Play();
			FeedbackAlert.Show("Not implemented yet...");
		}

		// TASK : PLACEHOLDER
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

		public void Quit()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}

		#endregion

		#endregion
	}
}

