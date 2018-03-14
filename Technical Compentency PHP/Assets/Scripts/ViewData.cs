using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewData : MonoBehaviour {

	public Button viewButton;
	public Text displayText;

	private float timer;
	private string serverData;

	public void buttonPressed(){
		StartCoroutine (pullDataServer ());
	}

	// Use this for initialization
	public IEnumerator pullDataServer () {
		WWW questionsData = new WWW ("http://41melquizgame.xyz/ass1/view.php");
		Debug.Log ("trying server");
		Debug.Log ("Just hold em off for a few seconds");
		while (!questionsData.isDone) { //starts a timer to stop trying to establish connection to server
			if (timer > 30) {
				Debug.LogError ("time out error on my server");
				break;
			} 
			timer += Time.deltaTime;
			yield return null;
		}

		if (!questionsData.isDone || questionsData.error != null) {
			//if we cannot connect to the server or there is some error in the data, check the prefs for previously saved questions
			Debug.Log (questionsData.error);
			Debug.Log ("Failed to hit my server");
		} 
		else { //we got the string from the server
			Debug.Log ("data recieved");
			Debug.Log (questionsData.text);
			serverData = questionsData.text;
			displayText.text = serverData;
			yield return questionsData;

		}
	}
	

}
