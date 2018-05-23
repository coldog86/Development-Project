using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using UnityEngine.Audio;
using UnityEngine.Events;

namespace _LetsQuiz
{
	public class SubmitScore : MonoBehaviour {


		private PlayerController _playerController;

		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		public void SubmitScores(string _name, int _playerScore)
		{
			StartCoroutine(Submit(_name, _playerScore));
		}
		
		private IEnumerator Submit(string username, int score)
	        {
	            WWWForm form = new WWWForm();
				float _connectionTimer = 0.0f;
				float _connectionTimeLimit = 1000000.0f;
				string _playerString = "";
				string s = score.ToString();


	            form.AddField("usernamePost", username);
	            form.AddField("scorePost", s);

			WWW loginRequest = new WWW(ServerHelper.Host + ServerHelper.SubmitHighScore, form);

	            while (!loginRequest.isDone)
	            {
				 _connectionTimer += Time.deltaTime;

	                if (_connectionTimer > _connectionTimeLimit)
	                {
	                    FeedbackAlert.Show("Server time out.");
	                    Debug.LogError("DataController : Login() : " + loginRequest.error);
	                    yield return null;
	                }

	                // extra check just to ensure a stream error doesn't come up
	                if (_connectionTimer > _connectionTimeLimit || loginRequest.error != null)
	                {
	                    FeedbackAlert.Show("Server error.");
	                    Debug.LogError("DataController : Login() : " + loginRequest.error);
	                    yield return null;
	                }    
	            }

	            if (loginRequest.error != null)
	            {
	                FeedbackAlert.Show("Connection error. Please try again.");
	                Debug.Log("DataController : Submit() : " + loginRequest.error);
	                yield return null;
	            }

	            if (loginRequest.isDone)
	            {
	            	Debug.Log("score submitted");    
            	    yield return loginRequest;
            	    DestroyObject(gameObject);

	                    
	            }
	        }
	}
}
