using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using System.Collections.Generic;
using Random = UnityEngine.Random;
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
		public GameObject questionDisplay;
		public QuestionData questionData;

        public Text scoreText;
		public PlayerController player;


		[Header("Answers")]
		public SimpleObjectPool answerButtonObjectPool;
		public Transform answerButtonParent;

		public GameObject roundEndDisplay;

		[Header ("Controllers")]
		private DataController _dataController;
		private RoundData _currentRoundData;
		private QuestionController _questionController;

		private bool _isRoundActive;
		private int _questionIndex;
		private int _playerScore;

        private FeedbackClick _click;
        private FeedbackMusic _music;


        private float _timeRemaining = 20;
		private QuestionData[] questionPool;
		public QuestionData _currentQuestion;

		private int numberOfQuestionsAsked;
		public bool clicked {get; set;}
		bool isCorrect = false;
		AnswerButton correctAnswerButton;
		AnswerData correctAnswerData;

		private List<GameObject> answerButtonGameObjects = new List<GameObject>();
		AnswerButton userSelection;



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
			questionPool = _questionController.getAllQuestionsAllCatagories(); 

			ShowQuestion ();
        }

        private void Update()
        {
            _timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (_timeRemaining <= 0)
                EndRound();
        }

        #endregion
        #region display question & answers

		public void ShowQuestion ()
		{	
			clicked = false;
			RemoveAnswerButtons ();
			QuestionData currentQuestionData = null;
			if (questionPool.Length <= numberOfQuestionsAsked) { //if all questions are asked, end round
				Debug.Log ("out of questions");
				EndRound ();
			} else {
				int randomNumber = Random.Range (0, questionPool.Length - 1); //gets random number between 0 and total number of questions
				currentQuestionData = questionPool [randomNumber];// Get the QuestionData for the current question
				questionText.text = currentQuestionData.questionText;  // Update questionText with the correct text
				_questionController.addAskedQuestionToAskedQuestions (currentQuestionData);//keep track of the questions we asked so we can repeat it for the oppoent player
				numberOfQuestionsAsked++;
				ShowAnswers(currentQuestionData);
			}			
		}

		private void ShowAnswers(QuestionData currentQuestionData)
		{
			
			List<int> answerText = new List<int> ();
			Random rnd = new Random ();
			for (int i = 0; i < currentQuestionData.answers.Length; i++) {  // For every AnswerData in the current QuestionData...
			
				int n = Random.Range (0, currentQuestionData.answers.Length);
				while (answerText.Contains (n)) {
					n = Random.Range (0, currentQuestionData.answers.Length);//randomise where the answers are displayed
				}
				answerText.Add (n);
				GameObject answerButtonGameObject = answerButtonObjectPool.GetObject ();// Spawn an AnswerButton from the object pool
				answerButtonGameObjects.Add (answerButtonGameObject);

				answerButtonGameObject.transform.SetParent (answerButtonParent);
				answerButtonGameObject.transform.localScale = Vector3.one; //I was having an issue were the scale blew out, this fixed it...
				AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton> ();
				answerButton.SetUp (currentQuestionData.answers [n]);// Pass the AnswerData to the AnswerButton to check if it correct

				if (answerButton.isCorrect (currentQuestionData.answers [n])) {
					correctAnswerButton = answerButton;
					isCorrect = true;
					correctAnswerData = currentQuestionData.answers [n];
				}
			}
		}

		#endregion

		public AnswerButton getCorrectAnswerButton() //getter used by the answerButton, I got an error when i tryed to declar with the variable at the top
		{
			return correctAnswerButton;
		}


		void RemoveAnswerButtons ()  // Return all spawned AnswerButtons to the object pool
		{
			while (answerButtonGameObjects.Count > 0) {
				answerButtonObjectPool.ReturnObject (answerButtonGameObjects [0]);
				answerButtonGameObjects.RemoveAt (0);
			}
		}

		#region like & dislike buttons
        // NOTE : placeholder
        public void ReportQuestion()
        {
            _click.Play();
            FeedbackAlert.Show("Report question");
        }

        // NOTE : placeholder
        public void LikeQuestion ()
		{
			_click.Play ();
			FeedbackAlert.Show ("Like question");
		}
		#endregion

		#region user stuff

		public void Score()
    	{
			if (!isCorrect) {
				Debug.Log ("Incorrect");
				Debug.Log("-5");
			}
			if (isCorrect){
				Debug.Log ("Correct");
				Debug.Log("+10");
        	}
			ShowQuestion();
    	}

		#endregion

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

        public void EndRound()
        {
            _music.Stop();
            SceneManager.LoadScene(BuildIndex.Result, LoadSceneMode.Single);
        }

        #endregion
    }
}
