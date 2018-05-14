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
		private AllQuestionData allQuestions;

		public void Load() {

			Debug.Log ("Load Question Controller");
			_playerController = FindObjectOfType<PlayerController>();

				questionData = _playerController.GetQuestionData();


		}

		public AllQuestionData extractQuestions() {

			AllQuestionData allQ = JsonUtility.FromJson<AllQuestionData>(questionData);
			return allQ;
		}

		public AllQuestionData getAllQuestions() {

			return allQuestions;
		}


	}
}