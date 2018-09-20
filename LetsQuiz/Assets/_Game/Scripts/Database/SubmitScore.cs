﻿using System.Collections;
using UnityEngine;

namespace _LetsQuiz
{
    public class SubmitScore : MonoBehaviour
    {
        #region variables

        private float _connectionTimer = 0.0f;
        private const float _connectionTimeLimit = 1000000.0f;

        private PlayerController _playerController;

        #endregion variables

        #region methods

        #region unity

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        #endregion unity

        #region submit specific

        public void SubmitScores(string name, int rightQuestions, int playerScore)
        {
            StartCoroutine(Submit(name, rightQuestions, playerScore));
        }

        private IEnumerator Submit(string username, int questionsRight, int score)
        {
            WWWForm form = new WWWForm();

            string s = score.ToString();

            form.AddField("usernamePost", username);
            form.AddField("scorePost", score);
            form.AddField("questionsCorrectPost", questionsRight);

            WWW submitRequest = new WWW(ServerHelper.Host + ServerHelper.SubmitHighScore, form);

            while (!submitRequest.isDone)
            {
                _connectionTimer += Time.deltaTime;

                if (_connectionTimer > _connectionTimeLimit)
                {
                    Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                    yield return null;
                    FeedbackAlert.Show("Server time out.");
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || submitRequest.error != null)
                {
                    Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                    yield return null;
                    FeedbackAlert.Show("Server error.");
                }
            }

            if (submitRequest.error != null)
            {
                Debug.Log("SubmitScore : Submit() : " + submitRequest.error);
                yield return null;
                FeedbackAlert.Show("Connection error. Please try again.");
            }

            if (submitRequest.isDone)
            {
                Debug.Log("Score submitted");
                yield return submitRequest;
                DestroyObject(gameObject);
            }
        }

        #endregion submit specific

        #endregion methods
    }
}