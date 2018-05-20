using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace _LetsQuiz
{
	public class HighscoreController : MonoBehaviour
	{

		[Header("Components")]
		private PlayerController _playerController;
		private string highScoreData;
		private HighScoreData allHighscores;

		public void Load() {

			Debug.Log ("Load HighScore Controller");
			_playerController = FindObjectOfType<PlayerController>();
			highScoreData = _playerController.GetHighScoreJSON();
		}

		public HighScoreData extractHighScores() {

			HighScoresContainer allHighScorers = JsonUtility.FromJson<HighScoresContainer> (highScoreData);
			return allHighScorers;
		}



	}
}

