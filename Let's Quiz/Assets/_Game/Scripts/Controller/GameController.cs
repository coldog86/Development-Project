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

        [Header("Color")]
        public Color timerMin;
        public Color timerMid;
        public Color timerMax;

        private float _timeRemaining = 20;

        [Header("Score")]
        public Text scoreDisplay;

        [Header("Question")]
        public Text questionText;
        public GameObject questionDisplay;
        public QuestionData questionData;
        public PlayerController player;
        public QuestionData currentQuestion;

        private QuestionData[] _questionPool;
        private int _numberOfQuestionsAsked;

        [Header("Answers")]
        public SimpleObjectPool answerButtonObjectPool;
        public Transform answerButtonParent;

        [Header("Controllers")]
        private DataController _dataController;
        private RoundData _currentRoundData;
        private QuestionController _questionController;
        private PlayerController _playerController;
        private SubmitScore _submitScore;

        private bool _isRoundActive;
        //private int _questionIndex;
        private FeedbackClick _click;
        private FeedbackMusic _music;

        public bool clicked { get; set; }

        private bool isCorrect = false;
        private AnswerButton correctAnswerButton;
        private AnswerData correctAnswerData;

        private List<GameObject> answerButtonGameObjects = new List<GameObject>();
        private AnswerButton userSelection;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            _click = FindObjectOfType<FeedbackClick>();
            _music = FindObjectOfType<FeedbackMusic>();
            _questionController = FindObjectOfType<QuestionController>();
            _playerController = FindObjectOfType<PlayerController>();
            _playerController.userScore = 0;

            _questionController.Load();
            _questionPool = _questionController.getAllQuestionsAllCatagories(); 

            ShowQuestion();
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

        public void ShowQuestion()
        {	
            scoreDisplay.text = _playerController.userScore.ToString();
            clicked = false;
            RemoveAnswerButtons();

            QuestionData currentQuestionData = null;
            if (_questionPool.Length <= _numberOfQuestionsAsked)
            { //if all questions are asked, end round
                Debug.Log("out of questions");
                EndRound();
            }
            else
            {
                int randomNumber = Random.Range(0, _questionPool.Length - 1); //gets random number between 0 and total number of questions
                currentQuestionData = _questionPool[randomNumber];// Get the QuestionData for the current question
                questionText.text = currentQuestionData.questionText;  // Update questionText with the correct text
                _questionController.addAskedQuestionToAskedQuestions(currentQuestionData);//keep track of the questions we asked so we can repeat it for the oppoent player
                _numberOfQuestionsAsked++;
                ShowAnswers(currentQuestionData);
            }			
        }

        private void ShowAnswers(QuestionData currentQuestionData)
        {
			
            List<int> answerText = new List<int>();
            Random rnd = new Random();
            for (int i = 0; i < currentQuestionData.answers.Length; i++)
            {  // For every AnswerData in the current QuestionData...
			
                int n = Random.Range(0, currentQuestionData.answers.Length);
                while (answerText.Contains(n))
                {
                    n = Random.Range(0, currentQuestionData.answers.Length);//randomise where the answers are displayed
                }
                answerText.Add(n);
                GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();// Spawn an AnswerButton from the object pool
                answerButtonGameObjects.Add(answerButtonGameObject);

                answerButtonGameObject.transform.SetParent(answerButtonParent);
                answerButtonGameObject.transform.localScale = Vector3.one; //I was having an issue were the scale blew out, this fixed it...
                AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();	
                answerButton.SetUp(currentQuestionData.answers[n]);// Pass the AnswerData to the AnswerButton to check if it correct

                if (answerButton.isCorrect(currentQuestionData.answers[n]))
                {
                    correctAnswerButton = answerButton;
                    isCorrect = true;
                    correctAnswerData = currentQuestionData.answers[n];
                }
            }
        }

        #endregion

        public AnswerButton getCorrectAnswerButton() //getter used by the answerButton, I got an error when i tryed to declar with the variable at the top
        {
            return correctAnswerButton;
        }


        void RemoveAnswerButtons()  // Return all spawned AnswerButtons to the object pool
        {
            while (answerButtonGameObjects.Count > 0)
            {
                answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
                answerButtonGameObjects.RemoveAt(0);
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
        public void LikeQuestion()
        {
            _click.Play();
            FeedbackAlert.Show("Like question");
        }

        #endregion

        #region user stuff

        public void Score(bool answer)
        {
            Debug.Log("score called bool = " + answer);
            if (answer)
                _playerController.userScore = _playerController.userScore + 10;

            if (!answer)
                _playerController.userScore = _playerController.userScore - 5;

            ShowQuestion();
        }

        #endregion

        #region timer specific

        private void UpdateTimeRemainingDisplay()
        {
            var timeRemaining = Mathf.Round(_timeRemaining);
            timerBar.value = timeRemaining;

            if (timerBar.value > 15)
                timerImage.color = timerMax;
            else if (timerBar.value < 16 && timerBar.value > 6)
                timerImage.color = timerMid;
            else if (timerBar.value <= 5)
                timerImage.color = timerMin;
        }

        #endregion

        public void EndRound()
        {
            _music.Stop();

            if (_playerController.userScore > _playerController.GetHighestScore())
            {
                Debug.Log("new high score");
                _playerController.scoreStatus = "new high score";
                _playerController.SetHighestScore(_playerController.userScore);
                _submitScore = FindObjectOfType<SubmitScore>();
                _submitScore.SubmitScores(_playerController.GetUsername(), _playerController.GetHighestScore());
            }
            else
            {
                _playerController.scoreStatus = "no change";
            }

            SceneManager.LoadScene(BuildIndex.Result, LoadSceneMode.Single);
            Debug.Log("end round");
            Debug.Log(_playerController.scoreStatus);
        }

        #endregion
    }
}
