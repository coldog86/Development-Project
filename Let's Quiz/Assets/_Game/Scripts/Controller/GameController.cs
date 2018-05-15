using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using System.Collections.Generic;
using Random = UnityEngine.Random;

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;


namespace _LetsQuiz
{
    public class GameController : MonoBehaviour
    {
        #region variables

        [Header("Timer")]
        public Slider timerBar;
        public Image timerImage;
        public Color timerColorMax;
        public Color timerColorMid;
        public Color timerColorMin;

        [Header("Question")]
        public Text questionText;
        public Text scoreText;
		public PlayerController player;
		public AllQuestions allQuestions;
		public QuestionData questionData;
		public Text answerText1;
		public Text answerText2;
		public Text answerText3;
		public Text answerText4;

		[Header("Answer Buttons")]
		public Button answerButton1;
		public Button answerButton2;
		public Button answerButton3;
		public Button answerButton4;

		public Transform answerButtonParent;
		public GameObject questionDisplay;
		public GameObject roundEndDisplay;

		private DataController _dataController;
		private RoundData _currentRoundData;
		private QuestionData[] questionPool;
		public QuestionData _currentQuestion;
		private QuestionController _questionController;

		private bool _isRoundActive;
		private int _questionIndex;
		private int _playerScore;


		private Button theButton;
		private ColorBlock theColor;

        private FeedbackClick _click;
        private FeedbackMusic _music;
        private float _timeRemaining = 20;

		int numberOfQuestionsAsked;



        #endregion

        #region methods

        #region unity

        private void Start()
        {
            _click = FindObjectOfType<FeedbackClick>();
            _music = FindObjectOfType<FeedbackMusic>();
			_questionController = FindObjectOfType<QuestionController>();

			//allQuestions.SetUp();

			_questionController.Load ();
			//questionPool = _questionController.extractQuestions ();
			//_questionPool = _currentRoundData.questions;
			questionPool = _questionController.getAllQuestionsAllCatagories();




			ShowQuestion ();




        }

        private void Update()
        {
            _timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (_timeRemaining <= 0)
                EndGame();
        }

        #endregion

        // NOTE : placeholder
        public void ReportQuestion()
        {
            _click.Play();
            FeedbackAlert.Show("Report question");
        }

        // NOTE : placeholder
        public void LikeQuestion()
        {
            _click.Play();
            FeedbackAlert.Show("Like question");


        }


		public void selectAnswer(Text answerText) 
		{
			_click.Play ();

			//determine if answer is correct, pass data onto AnswerButtonClicked

			AnswerData selectedAnswer = findQuestion (answerText);

			if (selectedAnswer.isCorrect) {
				FeedbackAlert.Show ("correct");
				//add points


			} else {
				FeedbackAlert.Show ("Incorrect");
				//remove points?

			}

			//show next question
			_questionIndex++;
			ShowQuestion ();

		}

		public AnswerData findQuestion(Text AnswerText) {

			for (int i = 0; i <= 3; i++) {
				
				if (AnswerText.text == _currentQuestion.answers[i].answerText) {

					return _currentQuestion.answers[i];
				}


			}
			return _currentQuestion.answers[0];
		}
			

		private void ShowQuestion ()
		{
			//retrieve next question


			//update UI
			if (questionPool.Length <= numberOfQuestionsAsked) { //if all questions are asked, end round
				Debug.Log ("out of questions");
				//TODO call somesort of endRound()
			} else {

				int randomNumber = Random.Range (0, questionPool.Length - 1); //gets random number between 0 and total number of questions



				_currentQuestion = questionPool [randomNumber];// Get the QuestionData for the current question
				questionText.text = _currentQuestion.questionText;  // Update questionText with the correct text
				_questionController.addAskedQuestionToAskedQuestions (_currentQuestion);//keep track of the questions we asked so we can repeat it for the oppoent player
				_questionController.removeFromAllQuestionsLeft (_currentQuestion);

				List<string> answers = new List<string>();
				for(int i =0; i<_currentQuestion.answers.Length; i++)
				{
					answers.Add(_currentQuestion.answers[i].answerText);
				}

				//this would be better if we were using the object pool to create the answer buttons

				randomNumber = Random.Range (0, answers.Count);
				answerText1.text = _currentQuestion.answers[randomNumber].answerText;
				answers.RemoveAt(randomNumber);

				randomNumber = Random.Range (0, answers.Count);
				answerText2.text = _currentQuestion.answers[randomNumber].answerText;
				answers.RemoveAt(randomNumber);

				randomNumber = Random.Range (0, answers.Count);
				answerText3.text = _currentQuestion.answers[randomNumber].answerText;
				answers.RemoveAt(randomNumber);

				randomNumber = Random.Range (0, answers.Count);
				answerText4.text = _currentQuestion.answers[randomNumber].answerText;
				answers.RemoveAt(randomNumber);

			

				numberOfQuestionsAsked++;
				questionPool = _questionController.removeQuestion(questionPool, randomNumber);
			}
		}




        #region timer specific

        private void UpdateTimeRemainingDisplay()
        {
            var timeRemaining = Mathf.Round(_timeRemaining);
            timerBar.value = timeRemaining;

            if (timerBar.value > 15)
                timerImage.color = timerColorMax;
            else if (timerBar.value < 16 && timerBar.value > 6)
                timerImage.color = timerColorMid;
            else if (timerBar.value <= 5)
                timerImage.color = timerColorMin;
        }

        #endregion

        public void EndGame()
        {
            _music.Stop();
            SceneManager.LoadScene(BuildIndex.Result, LoadSceneMode.Single);
        }

        #endregion
    }
}

