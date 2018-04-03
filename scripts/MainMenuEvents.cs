using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuEvents : MonoBehaviour {
	private Text signInButtonText;
	private Text authStatus;

	// Use this for initialization
	void Start () {
		GameObject startButton = GameObject.Find("startButton");
		EventSystem.current.firstSelectedGameObject = startButton;

		// Create client configuration
		PlayGamesClientConfiguration config = new 
			PlayGamesClientConfiguration.Builder()
			.Build();

		// Enable debugging output (recommended)
		PlayGamesPlatform.DebugLogEnabled = true;

		// Initialize and activate the platform
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.Activate();

		signInButtonText =
			GameObject.Find("signInButton").GetComponentInChildren<Text>();
		authStatus = GameObject.Find("authStatus").GetComponent<Text>();

		PlayGamesPlatform.Instance.Authenticate(SignInCallback, true);
	}

	public void SignIn()
	{
		if (!PlayGamesPlatform.Instance.localUser.authenticated) {
			// Sign in with Play Game Services, showing the consent dialog
			// by setting the second parameter to isSilent=false.
			PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
		} else {
			// Sign out of play games
			PlayGamesPlatform.Instance.SignOut();

			// Reset UI
			signInButtonText.text = "Sign In";
			authStatus.text = "";
		}
	}

	public void SignInCallback(bool success) {
		if (success) {
			Debug.Log("(Quiz) Signed in!");

			// Change sign-in button text
			signInButtonText.text = "Sign out";

			// Show the user's name
			authStatus.text = "Signed in as: " + Social.localUser.userName;
		} else {
			Debug.Log("(Quiz) Sign-in failed...");

			// Show failure message
			signInButtonText.text = "Sign in";
			authStatus.text = "Sign-in failed";
		}
	}
}