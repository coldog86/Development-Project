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


		// Use this for initialization
		void Start () {

			_CheckForOpenGames = FindObjectOfType<CheckForOpenGames> ();
			_MenuController = FindObjectOfType<MenuController>();

			Debug.Log("checking for open games");
			_CheckForOpenGames.CheckForGamesNeedingOpponents ();


		}

		public void StartOpenGame(OngoingGamesData _ongoingGameData)
		{
			Debug.Log(_ongoingGameData.askedQuestions);
			
		}



        public void StartGametest()
        {
        	_MenuController.StartGame();
        }

		

	}
}

