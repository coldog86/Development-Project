using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace _LetsQuiz
{
	[System.Serializable]
	public class AllQ {

		public List<CategoryQuestions> categories;

	}
	[System.Serializable]
	public class CategoryQuestions{

		public List<QuestionData> categoryQuestions;

	}
}
