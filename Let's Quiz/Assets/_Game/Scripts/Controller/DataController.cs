using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Configuration;

namespace _LetsQuiz
{
	//Persistant class created on game initialisation 
	public class DataController : MonoBehaviour
	{
		#region variables

		public bool serverStatus { get; set; }
		public string allQuestionJSON { get; set; }

		private SettingsController _settingsController;
		private LoadHelper _loadHelper;
		private float _connectionTimer = 0.0f;

		#endregion

		#region methods

		#region unity

		private void Start()
		{
			DontDestroyOnLoad(this.gameObject);
			//TODO sort these out
			/*
            _loadHelper = GetComponent<LoadHelper>();
            _settingsController = GetComponent<SettingsController>();
            _settingsController.LoadPlayerSettings();
            */

			GetAllQuestions getAllQuestions = FindObjectOfType<GetAllQuestions>();
			StartCoroutine(getAllQuestions.pullAllQuestionsFromServer()); ;
		}

		private void Quit()
		{
			Application.Quit();

			// NOTE : DEBUG PURPOSES ONLY
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
		}

		private void ShowModal(string message)
		{
			//TODO (col) I commented this out I took the server connection stuff out of the data controller and put it in its own class
			//I have not included a RetryConnection as we never 'connect' to the 'server', we only query an SQL table for info
			//FeedbackTwoButtonModal.Show("Error!", message + "\nDo you wish to retry?", "Yes", "No", RetryConnection, Quit);
		}

		#endregion

		#endregion

		public void init()
		{

		}
	}

}