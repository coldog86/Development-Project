using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace _LetsQuiz
{
	public class QuestionController : MonoBehaviour
	{
		[Header("Components")]
		private PlayerController _playerController;
		private string questionData;
		private GameData allQuestions;

		public void Load() {

			Debug.Log ("Load Question Controller");
			_playerController = FindObjectOfType<PlayerController>();
			questionData = _playerController.GetQuestionData();
		}

		public GameData extractQuestions() {

			GameData allQ = JsonUtility.FromJson<GameData>(questionData);
			return allQ;
		}

		public QuestionData[] extractQuestions(int catagory) {

			GameData allQ = JsonUtility.FromJson<GameData>(questionData);
			QuestionData[] allQuestionsInCatagory = allQ.allRoundData[catagory].questions;
			return allQuestionsInCatagory;
		}

		public GameData getAllQuestions() {

			return allQuestions;
		}

		public QuestionData[] getAllQuestionsAllCatagories()
		{
			List<QuestionData> questionsList = new List<QuestionData>();
			GameData allQ = JsonUtility.FromJson<GameData>(questionData);
			for(int i =0; i<allQ.allRoundData.Length; i++)
			{
				for(int n = 0; n<allQ.allRoundData[i].questions.Length; n++)
				{
					questionsList.Add(allQ.allRoundData[i].questions[n]);
				}
			}
			QuestionData[] allQuestionsAllCatagories = questionsList.ToArray();

			return allQuestionsAllCatagories;
		}

		public QuestionData[] removeQuestion(QuestionData[] questionPool, int remove)
		{
			List<QuestionData> poolAsList = new List<QuestionData>();


			for(int i = 0; i < questionPool.Length; i++)
				poolAsList.Add(questionPool[i]);

			poolAsList.RemoveAt (remove); 
			questionPool = poolAsList.ToArray ();
			return questionPool;
		}


		public void addAskedQuestionToAskedQuestions(QuestionData _currentQuestion)
		{
		//TODO when multi player is a thing we need to record the "player's" questions so we can ask the opponent 
		}

		public void removeFromAllQuestionsLeft(QuestionData _currentQuestion)
		{
			//TODO for multi player we will need to update the quetion pool so a user is not asked the same questions in different rounds of the same game
		}


	}
}