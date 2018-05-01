using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _LetsQuiz { 
	public class GetAllQuestions : MonoBehaviour {

		float _timer = 5; //time in seconds before attempt to hit server times out

		private DataController dataController;

		// Use this for initialization
		void Start () {
			dataController = FindObjectOfType<DataController>();
		}

		public IEnumerator pullAllQuestionsFromServer()
		{
			WWW questionsData = new WWW("http://41melquizgame.xyz/LQ/pullAllQuestions.php");
			Debug.Log("Just hold em off for a few seconds");
			while (!questionsData.isDone)
			{ //start a timer to stop trying to establish connection to server
				if (_timer < 0)
				{
					Debug.LogError("server time out");
					dataController.serverStatus = false;
					break;
				}
				_timer -= Time.deltaTime;
				yield return null;
			}

			if (!questionsData.isDone || questionsData.error != null)               
			{
				/*if we cannot connect to the server or there is some error in the data, 
                 * check the prefs for previously saved questions*/
				Debug.LogError(questionsData.error);
				Debug.Log("Failed to hit the server");
				dataController.serverStatus = false;               
			}
			else
			{ //we got the string from the server, it is every question in JSON format
				Debug.Log("Vox transmition recieved");
				Debug.Log(questionsData.text);
				dataController.serverStatus = true;
				dataController.allQuestionJSON = questionsData.text;
				yield return questionsData;

			}
			PlayerPrefs.SetString("savedQuestions", questionsData.text);
			dataController.init();
		}
	}

}

