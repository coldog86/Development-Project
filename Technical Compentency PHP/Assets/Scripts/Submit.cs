using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Submit : MonoBehaviour {

	public InputField inputField;
	private bool failed = false;

	public void updateDataBase(){
		Debug.Log("update database");
		WWWForm form = new WWWForm ();
		form.AddField ("dataPost", inputField.text);
		WWW www = new WWW ("http://41melquizgame.xyz/ass1/push.php", form); 

		float timer = 0;
		while (!www.isDone) { //starts a timer to stop trying to establish connection to server
			timer += Time.deltaTime;
			if (timer > 1000000) {				
				Debug.LogError ("time out error on my server");
				Debug.Log (www.error);	
				failed = true;
				break;
			}

		}
		if (www.isDone) {
			if(!failed)
				Debug.Log ("updated games");
			
		}

	}
}
