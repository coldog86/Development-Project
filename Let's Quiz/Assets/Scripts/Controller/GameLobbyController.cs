using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Collections;


namespace _LetsQuiz
{
	public class GameLobbyController : MonoBehaviour {

		CheckForOpenGames _CheckForOpenGames;
		OngoingGamesData _DataForExistingGame;
		MenuController _MenuController;
		PlayerController _playerController;
		CheckForPlayerExistingGames _checkForPlayerExistingGames;

		int[] gameNumbers;

		public Button ongoingGame1;
		public Button ongoingGame2;
		public Button ongoingGame3;
		public Button ongoingGame4;


		// Use this for initialization
		void Start () {

			_CheckForOpenGames = FindObjectOfType<CheckForOpenGames> ();
			_MenuController = FindObjectOfType<MenuController>();
			_checkForPlayerExistingGames = FindObjectOfType<CheckForPlayerExistingGames>();
			_playerController = FindObjectOfType<PlayerController>();

			//PlayerPrefs.SetString(_playerController.GetUsername(), "55194,26117,37969,39617");

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
			_CheckForOpenGames.CheckForGamesNeedingOpponents ();        	
        }

		

	}
}

