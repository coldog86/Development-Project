﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


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

        [Header("Score")]
        public Text scoreDisplay;

        [Header("Question")]
        public Text questionText;
        public GameObject questionDisplay;
        public QuestionData questionData;
        public PlayerController playerController;
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

        private float _timeRemaining = 30;

        private bool _isRoundActive;
        //private int _questionIndex;
        private FeedbackClick _click;
        private FeedbackMusic _music;

        private bool _isCorrect = false;
        private AnswerButton _correctAnswerButton;
        private AnswerData _correctAnswerData;

        private List<GameObject> _answerButtonGameObjects = new List<GameObject>();
        private AnswerButton _userSelection;

        #endregion

        #region properties

        public bool clicked { get; set; }

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

            //if all questions are asked, end round
            if (_questionPool.Length <= _numberOfQuestionsAsked)
            { 
                Debug.Log("GameController : Show Questions(): Out of Questions");
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

            // For every AnswerData in the current QuestionData...
            for (int i = 0; i < currentQuestionData.answers.Length; i++)
            {		
                int n = Random.Range(0, currentQuestionData.answers.Length);

                while (answerText.Contains(n))
                    n = Random.Range(0, currentQuestionData.answers.Length); //randomise where the answers are displayed

                answerText.Add(n);
                GameObject answerButtonGameObject = answerButtonObjectPool.GetObject(); // Spawn an AnswerButton from the object pool
                _answerButtonGameObjects.Add(answerButtonGameObject);

                answerButtonGameObject.transform.SetParent(answerButtonParent);
                answerButtonGameObject.transform.localScale = Vector3.one; //I was having an issue were the scale blew out, this fixed it...
                AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();	
                answerButton.SetUp(currentQuestionData.answers[n]); // Pass the AnswerData to the AnswerButton to check if it correct

                if (answerButton.isCorrect(currentQuestionData.answers[n]))
                {
                    _correctAnswerButton = answerButton;
                    _isCorrect = true;
                    _correctAnswerData = currentQuestionData.answers[n];
                }
            }
        }

        #endregion

        #region button set up

        public AnswerButton getCorrectAnswerButton() //getter used by the answerButton, I got an error when i tryed to declar with the variable at the top
        {
            return _correctAnswerButton;
        }

        private void RemoveAnswerButtons()  // Return all spawned AnswerButtons to the object pool
        {
            while (_answerButtonGameObjects.Count > 0)
            {
                answerButtonObjectPool.ReturnObject(_answerButtonGameObjects[0]);
                _answerButtonGameObjects.RemoveAt(0);
            }
        }

        #endregion

        #region like & dislike buttons

        // TASK : to be completed when multiplayer is implemented
        public void ReportQuestion()
        {
            _click.Play();
            FeedbackAlert.Show("Report question");
        }

        // TASK : to be completed when multiplayer is implemented
        public void LikeQuestion()
        {
            _click.Play();
            FeedbackAlert.Show("Like question");
        }

        #endregion

        #region scoring specific

        public void Score(bool answer)
        {
            Debug.Log("GameController : Score(): score called bool = " + answer);

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

        #region navigation specific

        public void EndRound()
        {
            _music.Stop();

            if (_playerController.userScore > _playerController.GetHighestScore())
            {
                Debug.Log("GameController : EndRound(): New High Score");
                _playerController.scoreStatus = "new high score";
                _playerController.SetHighestScore(_playerController.userScore);

                if (_playerController.GetPlayerType() == PlayerStatus.LoggedIn)
                {
                    _submitScore = FindObjectOfType<SubmitScore>();
                    _submitScore.SubmitScores(_playerController.GetUsername(), _playerController.GetHighestScore());
                }
            }
            else
            {
                Debug.Log("GameController : EndRound(): No Score Change");
                _playerController.scoreStatus = "no change";
            }
                

            SceneManager.LoadScene(BuildIndex.Result, LoadSceneMode.Single);

            Debug.Log("GameController : EndRound(): End of Round");
            Debug.Log(_playerController.scoreStatus);
        }

        #endregion

        #endregion
    }
}
