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
		private AllQ allQuestions;

		public void Load() {

			Debug.Log ("Load Question Controller");
			_playerController = FindObjectOfType<PlayerController>();

				questionData = _playerController.GetQuestionData();


		}

		public AllQ extractQuestions() {

			AllQ allQ = JsonUtility.FromJson<AllQ>(questionData);
			return allQ;
		}

		public AllQ getAllQuestions() {

			return allQuestions;
		}


	}
}