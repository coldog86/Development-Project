using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FacebookScript : MonoBehaviour {

	public GameObject DialogLoggedIn;
	public GameObject DialogLoggedOut;
	public GameObject DialogUsername;
	public GameObject DialogProfilePic;

	void Awake()
	{
		FacebookManager.Instance.InitFB();
		DealWithFBMenus (FB.IsLoggedIn);
	}

	public void FBlogin()
	{
		List<string> permissions = new List<string> ();
		permissions.Add ("public_profile");
		FB.LogInWithReadPermissions (permissions, AuthCallBack);
	}

	void AuthCallBack(IResult result)
	{
		if (result.Error != null) {
			Debug.Log (result.Error);
		} else {
			if (FB.IsLoggedIn) {
				FacebookManager.Instance.isLoggedIn = true;
				FacebookManager.Instance.GetProfile ();
				Debug.Log ("FB is logged in");
			} else {
				Debug.Log ("FB is not logged in");
			}

			DealWithFBMenus (FB.IsLoggedIn);
		}

	}

	void DealWithFBMenus(bool isLoggedIn)
	{
		if (isLoggedIn) {
			DialogLoggedIn.SetActive (true);
			DialogLoggedOut.SetActive (false);

			if (FacebookManager.Instance.profileName != null) {
				Text userName = DialogUsername.GetComponent<Text> ();
				userName.text = "Nello, " + FacebookManager.Instance.profileName;
			} else {
				StartCoroutine ("waitForProfileName");
			}

			if (FacebookManager.Instance.profilePic != null) {
				Image profilePic = DialogProfilePic.GetComponent<Image> ();
				profilePic.sprite = FacebookManager.Instance.profilePic;
			} else {
				StartCoroutine ("waitForProfilePic");
			}
		}else{
			DialogLoggedIn.SetActive (false);
			DialogLoggedOut.SetActive (true);
		}
	}

	IEnumerator waitForProfileName()
	{
		while (FacebookManager.Instance.profileName == null) {
			yield return null;
		}
		DealWithFBMenus (FB.IsLoggedIn);
	}

	IEnumerator waitForProfilePic()
	{
		while (FacebookManager.Instance.profilePic == null) {
			yield return null;
		}
		DealWithFBMenus (FB.IsLoggedIn);
	}


	public void share ()
	{
		FacebookManager.Instance.share ();
	}

	public void invite()
	{
		FacebookManager.Instance.invite ();
	}

	public void shareWithUsers()
	{
		FacebookManager.Instance.shareWithUsers ();
	}
}