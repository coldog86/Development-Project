using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

namespace _LetsQuiz{

	public class AnswerButton : MonoBehaviour 
	{
		public Text answerText;
		public Button answerButton;

		private AnswerData answerData;
		private GameController _GameController; 

		void Start()
		{
			_GameController = FindObjectOfType<GameController>();
		}

		public void SetUp(AnswerData data)
		{
			answerData = data;
			answerText.text = answerData.answerText;
		}

		public bool isCorrect(AnswerData data){
			bool isCorrect = data.isCorrect;
			return isCorrect;
		}

		public void HandleAnswerClick()
		{
			if(!_GameController.clicked) //prevents players from selecting multiple answers
			{
				_GameController.clicked = true;
				if (!answerData.isCorrect) 	
					changeToRed ();
			 	else 
					changeToGreen ();
			}

			
		
		}




		public void changeToGreen()
		{
			StartCoroutine("green");
		}
		IEnumerator green()
		{
			Color prevColor = GetComponent<Image> ().color; //get color that buttons are so we can change them back.
			GetComponent<Image> ().color = Color.green; //set wrong push to red
			yield return new WaitForSeconds(1); //give people a chance to see 
			GetComponent<Image> ().color = prevColor; //change color back
			_GameController.Score();
		}

		public void changeToRed()
		{
			StartCoroutine("red");
		}
		IEnumerator red()
		{
			Color prevColor = GetComponent<Image> ().color; //get color that buttons are so we can change them back.
			GetComponent<Image> ().color = Color.red; //set wrong push to red
			AnswerButton rightButton = _GameController.getCorrectAnswerButton();  //get the correct answer
			rightButton.GetComponent<Image> ().color = Color.green; //change it to green
			yield return new WaitForSeconds(1); //give people a chance to see 
			GetComponent<Image> ().color = prevColor; //change wrong answer color back 
			rightButton.GetComponent<Image> ().color = prevColor; //change correct answer color back
			_GameController.Score ();

		}


	}
}