using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

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
		private AllQuestionData questionPool;
		public QuestionData currentQuestion;
		private QuestionController questionController;

		private bool _isRoundActive;
		private int _questionIndex;
		private int _playerScore;


		private Button theButton;
		private ColorBlock theColor;

        private FeedbackClick _click;
        private FeedbackMusic _music;
        private float _timeRemaining = 20;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            _click = FindObjectOfType<FeedbackClick>();
            _music = FindObjectOfType<FeedbackMusic>();
			questionController = FindObjectOfType<QuestionController>();

			//allQuestions.SetUp();

			questionController.Load ();
		questionPool = questionController.extractQuestions ();
			//_questionPool = _currentRoundData.questions;

			_questionIndex = 0;

			for(int i = 0; i<questionPool.allRoundData.Length; i++ )
				Debug.Log (questionPool.allRoundData [i].name); //retrieve the name of category

			Debug.Log("length is....");
			Debug.Log (questionPool.allRoundData[0].questions.Length);


			Debug.Log (questionPool.allRoundData [0].questions[0].questionText); //retrieve questionText but returns null
			currentQuestion = questionPool.allRoundData [0].questions [_questionIndex];
			Debug.Log ("Current question" + currentQuestion.questionText);

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
				
				if (AnswerText.text == currentQuestion.answers[i].answerText) {

					return currentQuestion.answers[i];
				}


			}
			return currentQuestion.answers[0];
		}
			

		private void ShowQuestion()
		{
			//retrieve next question
			currentQuestion = questionPool.allRoundData[0].questions[_questionIndex];

			//update UI
			questionData.answers = currentQuestion.answers;
			answerText1.text = questionData.answers [0].answerText;
			answerText2.text = questionData.answers [1].answerText;
			answerText3.text = questionData.answers [2].answerText;
			answerText4.text = questionData.answers [3].answerText;
			questionText.text = currentQuestion.questionText;
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

