using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace _LetsQuiz
{
    public class GameLobbyController : MonoBehaviour
    {
        private CheckForOpenGames _CheckForOpenGames;
		private DataController _dataController;
        private MenuController _MenuController;
        private PlayerController _playerController;
        private CheckForPlayerExistingGames _checkForPlayerExistingGames;
		private QuestionController _questionController;
		public QuestionData[] questionsPoolFromCatagory { get; }

        private int[] gameNumbers;

		public GameObject catagoryPopUp;
		public Dropdown catagoryDD;
		public GameObject catAckPanel;
		public GameObject catSelectPanel;
		public GameObject backgroundStuff; 

        // Use this for initialization
        private void Start()
        {
			DontDestroyOnLoad (this.gameObject);
			_CheckForOpenGames = FindObjectOfType<CheckForOpenGames>();
            _MenuController = FindObjectOfType<MenuController>();
            _checkForPlayerExistingGames = FindObjectOfType<CheckForPlayerExistingGames>();
            _playerController = FindObjectOfType<PlayerController>();
			_questionController = FindObjectOfType<QuestionController> ();

			populateDropDown ();

            Debug.Log("here we go");
            if (PlayerPrefs.HasKey(_playerController.GetUsername()))
            {
                //Debug.Log(PlayerPrefs.GetString(_playerController.ongoingGamesKey));
                Debug.Log(PlayerPrefs.GetString(_playerController.GetUsername()));
                _checkForPlayerExistingGames.GetPlayersOpenGames();
            }
            else
                Debug.Log("player has no ongoing games");
        }

        public void StartOpenGame(OngoingGamesData _ongoingGameData)
        {
            Debug.Log(_ongoingGameData.askedQuestions);
        }

        public void StartNewGame()
        {
            Debug.Log("checking for open games");
            _CheckForOpenGames.CheckForGamesNeedingOpponents();
        }

        public void BackToMenu()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

		public void presentPopUp()
		{
			catagoryDD.gameObject.SetActive (true);
			if (_dataController.turnNumber == 1) 
			{
					
			}

		}

		private void populateDropDown()
		{
			List<string> catagories = new List<string> ();
			catagories = _questionController.getAllCatagories();
			catagoryDD.AddOptions (catagories);			
		}

		public void catagorySelected()
		{
			string catagory = (catagoryDD.options[catagoryDD.value]).text;
			Debug.Log ("catagory selected = " + catagory);
			QuestionData[] questionsFromCat = _questionController.getQuestionsFromSpecificCatagories (catagoryDD.value);
			//_dataController = FindObjectOfType<DataController> ();
			//_dataController._tempQuestionPool = questionsFromCat;
			_MenuController.StartGame();
		}

		public void catagoryAcknowledged()
		{
			_MenuController.StartGame();
		}

    }
}