using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Collections;

namespace _LetsQuiz
{
	public class SubmitGame : MonoBehaviour {

		PlayerController _playerController;
		QuestionController _questionController;
		DataController _dataController;
		string _questionPool;

		// Use this for initialization
		void Start () 
		{
			_playerController = FindObjectOfType<PlayerController>();
			_dataController = FindObjectOfType<DataController>();
			_questionController = FindObjectOfType<QuestionController>();

		}
		
		public void SubmitGameToDB(string _questionPool)
        {
			this._questionPool = _questionPool;
			StartCoroutine(SubmitRound1());
        }



		private IEnumerator SubmitRound1 ()
		{
			string address = "";
			WWWForm form = new WWWForm ();
			Debug.Log("test1");
			if (_dataController.turnNumber == 1) 
			{
				int rand = Random.Range (1, 100000);
				Debug.Log("test1");
				form.AddField ("gameNumberPost", rand); //TODO need a better way to generate unique game numbers for the first game
				form.AddField ("playerNamePost", _playerController.GetUsername ());
				form.AddField ("opponentNamePost", "not yet assigned");
				form.AddField ("askedQuestionsPost", _questionController.getAskedQuestions ());
				form.AddField ("QuestionsLeftInCatagoryPost", _questionPool);
				form.AddField ("Round1CatagoryPost", "catagories not yet set");
				form.AddField ("Round2CatagoryPost", "catagories not yet set");
				form.AddField ("playerScorePost", _playerController.userScore);
				form.AddField ("opponentScorePost", 0);
				form.AddField ("gameRequiresOppoentPost", 1);
				form.AddField ("turnsCompletedPost", _dataController.turnNumber);
				Debug.Log("test1");
				address = ServerHelper.Host + ServerHelper.SubmitRound1Data;
			}

			if (_dataController.turnNumber == 2) 
			{
				form.AddField ("gameNumberPost", _dataController.ongoingGameData.gameNumber); //TODO need a better way to generate unique game numbers for the first game
				form.AddField ("opponentNamePost", _playerController.GetUsername ());
				form.AddField ("Round2CatagoryPost", "catagories not yet set");
				form.AddField ("opponentScorePost", _playerController.userScore);
				form.AddField ("gameRequiresOppoentPost", 0);
				form.AddField ("turnsCompletedPost", _dataController.turnNumber);
	            
				address = ServerHelper.Host + ServerHelper.SubmitRound2Data;

			}
			WWW submitRequest = new WWW (address, form);
            while (!submitRequest.isDone)
            {
				float _connectionTimer = 0.0f;
		        const float _connectionTimeLimit = 1000000.0f;
				Debug.Log("test1");
                _connectionTimer += Time.deltaTime;
				Debug.Log("test1");
                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                    yield return null;
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || submitRequest.error != null)
				{Debug.Log("test1");
                    FeedbackAlert.Show("Server error.");
                    Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                    yield return null;
                }    
            }

            if (submitRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.Log("SubmitScore : Submit() : " + submitRequest.error);
                yield return null;
            }
			Debug.Log("test1");
            if (submitRequest.isDone)
            {
                Debug.Log("game data submitted");    
                yield return submitRequest;
                DestroyObject(gameObject); 
            }
        }

	}
}
