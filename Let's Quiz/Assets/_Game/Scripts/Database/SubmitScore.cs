using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace _LetsQuiz
{
    public class SubmitScore : MonoBehaviour
    {
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

            WWW submitRequest = new WWW(ServerHelper.Host + ServerHelper.SubmitHighScore, form);

            while (!submitRequest.isDone)
            {
                _connectionTimer += Time.deltaTime;

                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                    yield return null;
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || submitRequest.error != null)
                {
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

            if (submitRequest.isDone)
            {
                Debug.Log("Score submitted");    
                yield return submitRequest;
                DestroyObject(gameObject); 
            }
        }
    }
}
