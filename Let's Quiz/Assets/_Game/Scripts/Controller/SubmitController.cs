using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
	public class SubmitController : MonoBehaviour
	{

		#region variables

		[Header("Components")]
		public InputField questionInput;
		public InputField correctInput;
		public InputField wrong1Input;
		public InputField wrong2Input;
		public InputField wrong3Input;

		private FeedbackClick _click;
		private PlayerController _playerController;
		private GameObject _warningPanel;
		private GameObject _userPanel;

		#endregion

		#region methods

		public void checkInputs()
		{

			//charnes can you do your val
			bool _inputsOK = false;
			_inputsOK = true;
			if(_inputsOK)
				handleData();
			else
			{
				Debug.LogError("your data sucks and so do you, feel bad");
			}
		}


		private bool handleData()
		{
			Debug.Log ("attempting to submit");
			float _connectionTimer =0;
			float _connectionTimeLimit = 10000.0f;
			WWWForm form = new WWWForm();

			//form.AddField("usernamePost", _playerController.GetUsername());
			//form.AddField("idPost", _playerController.GetId());
			form.AddField("questionText", questionInput.text);
			form.AddField("correctAnswer", correctInput.text);
			form.AddField("wrong1", wrong1Input.text);
			form.AddField("wrong2", wrong2Input.text);
			form.AddField("wrong3", wrong3Input.text);
			form.AddField ("catgory", "User Submitted Question");

			WWW submitQuestion = new WWW("www.41melquizgame.xyz/LQ/submitQuestion.php", form);

			while (!submitQuestion.isDone)
			{ 
				_connectionTimer += Time.deltaTime;
				FeedbackAlert.Show("Attempting to submit question.");

				if (_connectionTimer > _connectionTimeLimit)
				{
					FeedbackAlert.Show("Time out error. Please try again.");
					Debug.LogError(submitQuestion.error);
					Debug.Log(submitQuestion.text);
					return false;
				}
			}

			if (submitQuestion.error != null)
			{
				FeedbackAlert.Show("Connection error. Please try again.");
				Debug.Log(submitQuestion.error);
				return false;
			}

			if (submitQuestion.isDone)
			{
				Debug.Log(submitQuestion.text);

				return true;
			}
			return false;

		}


		#endregion

		#region unity

		private void Awake()
		{
			_click = FindObjectOfType<FeedbackClick>();

			_playerController = FindObjectOfType<PlayerController>();

			_playerController.Load();

			_warningPanel = GameObject.FindGameObjectWithTag("Panel_Warning");
			//            _warningPanel.SetActive(false);

			_userPanel = GameObject.FindGameObjectWithTag("Panel_User");
			_userPanel.SetActive(false);
		}

		private void Start()
		{
			if (_playerController.GetPlayerType() != PlayerStatus.LoggedIn)
			{
				_warningPanel.SetActive(true);
				_userPanel.SetActive(false);
			}
			else
			{
				//              _warningPanel.SetActive(false);
				_userPanel.SetActive(true);
			}     
		}

		#endregion

		#region navigation specific

		public void BackToMenu()
		{
			_click.Play();
			SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
		}

		#endregion



	}
}
