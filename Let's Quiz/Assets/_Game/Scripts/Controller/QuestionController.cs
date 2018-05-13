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

		public string QuestionText = "empty question";
		public string CorrectAns = "Correct Answer";
		public string WrongAns1 = "Wrong Answer";
		public string WrongAns2 = "Wrong Answer";
		public string WrongAns3 = "Wrong Answer";

		public void Load() {
			_playerController = FindObjectOfType<PlayerController>();

			questionData = _playerController.GetQuestionData();


			string rawQ = extractQuestions ();
		}

		public string extractQuestions() {

			string rawQuestions = null;

			RawQuestion raw = JsonUtility.FromJson<RawQuestion>(questionData);
			Debug.Log(raw);

			return rawQuestions;
		}
			

	}
}