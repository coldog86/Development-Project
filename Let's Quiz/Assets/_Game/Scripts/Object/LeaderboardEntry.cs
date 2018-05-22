using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

namespace _LetsQuiz{

	public class LeaderboardEntry : MonoBehaviour 
	{

		public Text usernameText;
		public Text scoreText;

		private GameController _GameController; 

		void Start()
		{
			_GameController = FindObjectOfType<GameController>();
		}

		public void SetUp(string username, string score)
		{
			usernameText.text = username;
			scoreText.text = score;
			Debug.Log (usernameText.text);
		}



	}

}