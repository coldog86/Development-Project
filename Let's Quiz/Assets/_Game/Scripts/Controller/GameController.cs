using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Collections;

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

        private float _timeRemaining = 8; //TODO the in game slider does not work until the timer is 20 or less

        private bool _isRoundActive;

        //private int _questionIndex;
        private FeedbackClick _click;
        private FeedbackMusic _music;

        private bool _isCorrect = false;
        private AnswerButton _correctAnswerButton;
        private AnswerData _correctAnswerData;

        private List<GameObject> _answerButtonGameObjects = new List<GameObject>();
        private AnswerButton _userSelection;

        #endregion variables

        #region properties

        public bool clicked { get; set; }

        #endregion properties

        #region methods

        #region unity

        private void Start()
        {
            _click = FindObjectOfType<FeedbackClick>();
            _music = FindObjectOfType<FeedbackMusic>();
            _questionController = FindObjectOfType<QuestionController>();
            _playerController = FindObjectOfType<PlayerController>();
            _dataController = FindObjectOfType<DataController>();
            _playerController.AddToGamesPlayed();
            _playerController.userScore = 0;
            _questionController.Load(); //TODO what is this??

            _questionPool = GetQuestionPool();

            if (PlayerPrefs.HasKey(_playerController.GetUsername()))
            {
                string numbers = PlayerPrefs.GetString(_playerController.GetUsername());
                numbers = numbers + "," + _dataController.gameNumber;
                PlayerPrefs.SetString(_playerController.GetUsername(), numbers);
                Debug.Log("games in player prefs = " + PlayerPrefs.GetString(_playerController.GetUsername()));
            }
            else
            {
                PlayerPrefs.SetString(_playerController.GetUsername(), _dataController.gameNumber.ToString());
                Debug.Log("games in player prefs = " + PlayerPrefs.GetString(_playerController.GetUsername()));
            }
            ShowQuestion();
        }

        private void Update()
        {
            _timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (_timeRemaining <= 0)
                EndRound();
        }

        #endregion unity

        #region questionpool

        private QuestionData[] GetQuestionPool()
        {
            if (_dataController.turnNumber == 0)
            {
                //this should not actually happen as CheckForOpenGames() should have set the turn number to at least 1
                // treat this as a brand new game
                _questionPool = _questionController.getAllQuestionsAllCatagories();
                return _questionPool;
            }

            if (_dataController.turnNumber == 1 || _dataController.turnNumber == 3 || _dataController.turnNumber == 5)
            {
                //there is no open games, the user is the 'player' they will be starting a new game which will get an opponent later
                _questionPool = _questionController.getAllQuestionsAllCatagories();
                return _questionPool;
            }
            if (_dataController.turnNumber == 2 || _dataController.turnNumber == 4 || _dataController.turnNumber == 6)
            {
                //there is an open game, the user will be the 'oppponent' they will be playing round 2 with a predetermined questionpool
                string roundDataJSON = _dataController.ongoingGameData.askedQuestions;
                RoundData rd = JsonUtility.FromJson<RoundData>(roundDataJSON);
                _questionPool = rd.questions;
                return _questionPool;
            }
            else
            {
                Debug.Log("something went wrong");
                _questionPool = _questionController.getAllQuestionsAllCatagories();
                return _questionPool;
            }
        }

        #endregion questionpool

        #region display question & answers

        public void ShowQuestion()
        {
            scoreDisplay.text = _playerController.userScore.ToString();
            clicked = false;
            RemoveAnswerButtons();
            _playerController.AddToTotalQuestionsAnswered();

            QuestionData currentQuestionData = null;

            //if all questions are asked
            if (_questionPool.Length <= 0)
            {
                if (_dataController.turnNumber == 0 | _dataController.turnNumber == 1)
                {
                    //the user is the player so if they have finished the question pool they have answered all the questions in the catagory.
                    Debug.Log("GameController : Show Questions(): Out of Questions");
                    EndRound();
                }
                if (_dataController.turnNumber == 2)
                {
                    //the user is the oppenent so if they have finished the question pool they have answered all the questions asked of the player, go on to the remaining questions in the catagory

                    if (_dataController.ongoingGameData.QuestionsLeftInCatagory.Length < 5)
                        EndRound();

                    string roundDataJSON = _dataController.ongoingGameData.QuestionsLeftInCatagory;
                    Debug.Log("out of asked questons, asking remaining questions: " + _dataController.ongoingGameData.QuestionsLeftInCatagory);
                    _dataController.ongoingGameData.QuestionsLeftInCatagory = "";
                    RoundData rd = JsonUtility.FromJson<RoundData>(roundDataJSON);
                    _questionPool = rd.questions;
                    ShowQuestion();
                }
            }
            else
            { //ask the question
                if (_dataController.turnNumber == 0 | _dataController.turnNumber == 1 | _dataController.turnNumber == 3 | _dataController.turnNumber == 5)
                {
                    //if user is player then quesitons should be asked in random order
                    int randomNumber = Random.Range(0, _questionPool.Length - 1); //gets random number between 0 and total number of questions
                    currentQuestionData = _questionPool[randomNumber];// Get the QuestionData for the current question
                    _questionPool = _questionController.removeQuestion(_questionPool, randomNumber); //remove question from list
                    questionText.text = currentQuestionData.questionText;  // Update questionText with the correct text
                    _questionController.addAskedQuestionToAskedQuestions(currentQuestionData);//keep track of the questions we asked so we can repeat it for the oppoent player
                }
                if (_dataController.turnNumber == 2 | _dataController.turnNumber == 4 | _dataController.turnNumber == 6)
                {
                    //if user is oppoent questions should be asked in the same order they were asked of player
                    currentQuestionData = _questionPool[0];// Get the QuestionData for the current question
                    _questionPool = _questionController.removeQuestion(_questionPool, 0); //remove question from list
                    questionText.text = currentQuestionData.questionText;  // Update questionText with the correct text
                }
                _numberOfQuestionsAsked++;
                ShowAnswers(currentQuestionData);
            }
        }

        private void ShowAnswers(QuestionData currentQuestionData)
        {
            List<int> answerText = new List<int>();

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
                    _playerController.AddToNumberCorrectAnswers();
                    _correctAnswerButton = answerButton;
                    _isCorrect = true;
                    _correctAnswerData = currentQuestionData.answers[n];
                }
            }
        }

        #endregion display question & answers

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

        #endregion button set up

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

        #endregion like & dislike buttons

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

        #endregion scoring specific

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

        #endregion timer specific

        #region navigation specific

        public void EndRound()
        {
            _music.Stop();

            SubmitToOngoingGamesDB();
            /* //TODO do we need this?
			if (_playerController.userScore > _playerController.GetHighestScore ())
			{
				Debug.Log ("GameController : EndRound(): New High Score");
				_playerController.scoreStatus = "new high score";
				_playerController.SetHighestScore (_playerController.userScore);

				if (_playerController.GetPlayerType () == PlayerStatus.LoggedIn)
				{
					_submitScore = FindObjectOfType<SubmitScore> ();
					_submitScore.SubmitScores (_playerController.GetUsername (), _playerController.GetHighestScore ());
				}
			}
			else */
            {
                Debug.Log("GameController : EndRound(): No Score Change");
                _playerController.scoreStatus = "no change";
            }

            SceneManager.LoadScene(BuildIndex.Result, LoadSceneMode.Single);

            Debug.Log("GameController : EndRound(): End of Round");
            Debug.Log(_playerController.scoreStatus);
        }

        public void SubmitToOngoingGamesDB()
        {
            SubmitGame submitGame;
            submitGame = FindObjectOfType<SubmitGame>();
            submitGame.SubmitGameToDB(_questionController.getRemainingQuestions(_questionPool));
        }
	
	 	public void UpvoteButton()
	 	{
			Upvote _upvote;
			_upvote = FindObjectOfType<Upvote>();
			Debug.Log("****current question is: " + currentQuestionData.questionText);
			_upvote.Uvote(currentQuestionData);	
		}

		public void DownvoteButton()
	 	{
			Downvote _downvote;
			_downvote = FindObjectOfType<Downvote>();
			Debug.Log("****current question is: " + currentQuestionData.questionText);
			_downvote.Dvote(currentQuestionData);	
		}
        #endregion navigation specific

        #endregion methods
    }
}
