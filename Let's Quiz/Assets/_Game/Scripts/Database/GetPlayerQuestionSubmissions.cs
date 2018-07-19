using System.Collections;
using UnityEngine;

namespace _LetsQuiz
{
	public class GetPlayerQuestionSubmissions : MonoBehaviour
	{
		#region variables

		private float _downloadTimer = 5.0f;

		private DataController _dataController;
		private PlayerController _playerController;

		#endregion

		#region methods

		#region unity

		private void Start()
		{
			_dataController = FindObjectOfType<DataController>();
			_playerController = FindObjectOfType<PlayerController>();
			StartCoroutine (PullQuesionSubmitters ());
		}

		#endregion

		#region download specific

		public IEnumerator PullQuesionSubmitters()
		{
			WWW download = new WWW(ServerHelper.Host + ServerHelper.GetQuestionSubmissionStuff);
			while (!download.isDone)
			{
				if (_downloadTimer < 0)
				{
					Debug.LogError("Server time out.");
					_dataController.serverConnected = false;
					break;
				}
				_downloadTimer -= Time.deltaTime;

				yield return null;
			}

			if (!download.isDone || download.error != null)
			{
				/* if we cannot connect to the server or there is some error in the data, 
                 * check the prefs for previously saved questions */
				Debug.LogError(download.error);
				Debug.Log("Failed to hit the server.");
				_dataController.serverConnected = false;               
			}
			else
			{ 
				// we got the string from the server, it is every question in JSON format
				Debug.Log("Question Subbmission stuff = ");
				Debug.Log(download.text);
				handleData (download.text);
				yield return download;

			} 
		}


		private void handleData(string json)
		{
			QuestAndSubContainer qsc = new QuestAndSubContainer ();
			json = "{\"dataForQuestAndSub\":" + json + "}"; //you have to do this because you cannot serialize directly into an array, you need an object that holds the array
			qsc = JsonUtility.FromJson<QuestAndSubContainer> (json);//now we have an object that holds our array of questAndSub objects
			QuestAndSub[] _questAndSub = qsc.dataForQuestAndSub; //fuck off the container and there is your array of stuff

		}

		#endregion

		#endregion

	}
}

