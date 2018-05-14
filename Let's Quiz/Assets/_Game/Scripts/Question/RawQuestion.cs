using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace _LetsQuiz
{

	[System.Serializable]
	public class AllQuestionData {

		public List<CategoryQuestions> allRoundData;

	}


	[System.Serializable]
	public class CategoryQuestions{

		public List<QuestionData> questions;
		public string name;

	}
}
