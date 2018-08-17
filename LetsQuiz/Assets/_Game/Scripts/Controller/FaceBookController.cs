using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using System;
using Random = UnityEngine.Random;

public class FaceBookController : MonoBehaviour 
{
	
	private static FaceBookController instance;

	public static FaceBookController Instance
	{
		get {
			if (instance == null) {
				GameObject fBM = new GameObject ("FBManager");
				fBM.AddComponent<FaceBookController> ();
			}
			return instance;
		}	
	}

	public bool isLoggedIn{ get; set; }
	public string profileName{ get; set; }
	public Sprite profilePic{ get; set; }
	public string appLinkURL{ get; set; }
	public string uriPic{ get; set; }
	public string profileId{ get; set; }
	public string profileEmail{ get; set; }


	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		instance = this;
		isLoggedIn = true;
	}

	public void InitFB()
	{
		if (!FB.IsInitialized) 
		{
			FB.Init (SetInit, OnHideUnity);
		} else {
			isLoggedIn = FB.IsLoggedIn;
		}
	}

	void SetInit()
	{
		if (FB.IsLoggedIn) {
			Debug.Log ("FB is logged in");
			GetProfile ();
		} else {
			Debug.Log ("FB is not logged in");
		}
		isLoggedIn = FB.IsLoggedIn;
	}

	void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	public void GetProfile()
	{
		FB.API ("/me?fields=name", HttpMethod.GET, DisplayUsername);
		FB.API ("/me?fields=id", HttpMethod.GET, DisplayUserId);
		FB.API ("/me?fields=email", HttpMethod.GET, DisplayUserEmail);
		//FB.API ("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
		FB.GetAppLink (dealWithAppLink);

	}


	void DisplayUsername(IResult result)
	{
		if (result.Error == null) {
			profileName = "" + result.ResultDictionary ["name"];
			Debug.Log("Your Name: " + profileName);
		} else {
			Debug.Log (result.Error);
		}
	}

	void DisplayUserId(IResult result)
	{
		if (result.Error == null) {
			profileId = "" + result.ResultDictionary ["id"];
			Debug.Log("Your Id: " + profileId);
		} else {
			Debug.Log (result.Error);
		}
	}

	void DisplayUserEmail(IResult result)
	{
		if (result.Error == null) {
			profileEmail = "" + result.ResultDictionary ["email"];
			Debug.Log("Your Email: " + profileEmail);
		} else {
			Debug.Log (result.Error);
		}
	}


	//void DisplayProfilePic(IGraphResult result)
	//{
	//if (result.Texture != null) {
	//profilePic = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 ());
	//}
	//}

	void dealWithAppLink(IAppLinkResult result)
	{
		if (!string.IsNullOrEmpty (result.Url)) {
			appLinkURL = result.Url;

		}
	}

	public void share()
	{
		InitFB ();
		FB.FeedShare (
			string.Empty,
			new Uri ("https://developers.facebook.com/"),
			"Let's Quiz",
			"Come play",
			"Check out this game",
			new Uri ("https://www.facebook.com/photo.php?fbid=10217071733810202&set=a.3575468990089&type=3&theater"),
			string.Empty,
			shareCallBack

		);    
	}
	void shareCallBack(IResult result)
	{
		if (result.Cancelled) {
			Debug.Log ("Share Cancelled");
		} else if (!string.IsNullOrEmpty (result.Error)) {
			Debug.Log ("Error on share!");
		} else if (!string.IsNullOrEmpty (result.RawResult)) {
			Debug.Log ("Success on share");
		}
	}


}









