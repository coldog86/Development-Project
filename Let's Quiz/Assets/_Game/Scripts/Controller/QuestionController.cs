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
			_playerController = FindObjectOfType<PlayerController>();

			questionData = _playerController.GetQuestionData();

			allQuestions = extractQuestions ();

		}

		public AllQ extractQuestions() {

			AllQ allQ = JsonUtility.FromJson<AllQ> (questionData);
			Debug.Log (allQ.categories);
			return allQ;
		}
			

	}
}