﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _LetsQuiz
{
    public class GetAllQuestions : MonoBehaviour
    {
        #region variables

        private float _downloadTimer = 5.0f;

        private DataController _dataController;
        private PlayerController _playerController;

		private string _QuestionJSON;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            _dataController = FindObjectOfType<DataController>();
            _playerController = FindObjectOfType<PlayerController>();
        }

        #endregion

        #region download specific

        public IEnumerator PullAllQuestionsFromServer()
        {
            WWW download = new WWW(ServerHelper.Host + ServerHelper.DownloadQuestion);
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
                Debug.Log("Vox transmition recieved");
                Debug.Log(download.text);

                _dataController.serverConnected = true;
                _dataController.allQuestionJSON = download.text;

                yield return download;

                _playerController.SetQuestionData(download.text);
				_QuestionJSON = download.text;
                _dataController.Init();
            } 
        }

		public string getJSON()
		{
			return _QuestionJSON;
		}


        #endregion

        #endregion

    }
}
