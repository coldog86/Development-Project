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
		public QuestionData[] questionsPoolFromCatagory { get; set;}
		public string catagoryText { get; set; }

        private int[] gameNumbers;

		public GameObject catagoryPopUp;
		public Dropdown catagoryDD;
		public GameObject catAckPanel;
		public GameObject catSelectPanel;
		public GameObject backgroundStuff;
		public Text catagoryText; 

        // Use this for initialization
        private void Start()
        {
			DontDestroyOnLoad (this.gameObject);
			_CheckForOpenGames = FindObjectOfType<CheckForOpenGames>();
            _MenuController = FindObjectOfType<MenuController>();
            _checkForPlayerExistingGames = FindObjectOfType<CheckForPlayerExistingGames>();
            _playerController = FindObjectOfType<PlayerController>();
			_questionController = FindObjectOfType<QuestionController> ();
			_dataController = FindObjectOfType<DataController> ();

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
			backgroundStuff.SetActive (false);
			catagoryPopUp.SetActive (true);
			if (_dataController.turnNumber == 1 || _dataController.turnNumber == 3) 
			{
				catSelectPanel.SetActive (true);
				populateDropDown ();
			}
			if (_dataController.turnNumber == 2 || _dataController.turnNumber == 4) 
			{
				catAckPanel.SetActive (true);
				Debug.Log (_dataController.ongoingGameData.Round1Catagory);
				string catagory = _questionController.getAllCatagories () [Convert.ToInt32(_dataController.ongoingGameData.Round1Catagory)] ;
				catagoryText.text = catagory;
			}
			if (_dataController.turnNumber == 5 || _dataController.turnNumber == 6) 
			{
				catAckPanel.SetActive (true);
				Debug.Log (_dataController.ongoingGameData.Round1Catagory);
				string catagory = _questionController.getAllCatagories () [Convert.ToInt32(_dataController.ongoingGameData.Round1Catagory)] ;
				catagoryText.text = catagory;
			}

		}

		private void populateDropDown()
		{
			List<string> catagories = new List<string> ();
			catagories = _questionController.getAllCatagories();
			if (_dataController.turnNumber == 3)
				catagories = _questionController.removeCatagory (catagories, Convert.ToInt32 (_dataController.ongoingGameData.Round1Catagory));
			catagoryDD.AddOptions (catagories);			
		}

		public void catagorySelected()
		{
			string catagory = (catagoryDD.options[catagoryDD.value]).text;
			Debug.Log ("catagory selected = " + catagory);
			questionsPoolFromCatagory = _questionController.getQuestionsFromSpecificCatagories (catagoryDD.value);
			_dataController.catagory = catagoryDD.value;
			_MenuController.StartGame();
		}

		public void catagoryAcknowledged()
		{
			_MenuController.StartGame();
		}

		

    }
}