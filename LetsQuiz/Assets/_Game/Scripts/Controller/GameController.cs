using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        public QuestionData currentQuestion;
        public QuestionData currentQuestionData;

        private QuestionData[] _questionPool;
        private int _numberOfQuestionsAsked;

        [Header("Answers")]
        public SimpleObjectPool answerButtonObjectPool;
        public Transform answerButtonParent;

        [Header("Controllers")]
        private RoundData _currentRoundData;
        private SubmitScore _submitScore;

        [Header("Other")]
        private Upvote _upVote;
        private Downvote _downVote;

        private float _timeRemaining = 8;

        //TODO the in game slider does not work until the timer is 20 or less
        private bool _isRoundActive;

        //private int _questionIndex;
        private FeedbackMusic _music;

        private bool _isCorrect = false;
        private AnswerButton _correctAnswerButton;
        private AnswerData _correctAnswerData;

        private List<GameObject> _answerButtonGameObjects = new List<GameObject>();
        private AnswerButton _userSelection;

        #endregion variables

        #region properties

        public DataController DataController
        {
            get
            {
                if (DataController.Initialised)
                    return DataController.Instance;
                else return null;
            }
        }

        public PlayerController PlayerController
        {
            get
            {
                if (PlayerController.Initialised)
                    return PlayerController.Instance;
                else return null;
            }
        }

        public QuestionController QuestionController
        {
            get
            {
                if (QuestionController.Initialised)
                    return QuestionController.Instance;
                else return null;
            }
        }

        public GameLobbyController GameLobbyController
        {
            get
            {
                if (GameLobbyController.Initialised)
                    return GameLobbyController.Instance;
                else return null;
            }
        }

        public bool clicked { get; set; }

        #endregion properties

        #region methods

        #region unity

        private void Awake()
        {
            _music = FindObjectOfType<FeedbackMusic>();
            _music.PlayGameMusic();
        }

        private void Start()
        {
            _upVote = FindObjectOfType<Upvote>();
            _downVote = FindObjectOfType<Downvote>();

            PlayerController.AddToGamesPlayed();
            PlayerController.UserScore = 0;
            QuestionController.Load();

            _questionPool = GetQuestionPool();

            Destroy(GameLobbyController);

			if (PlayerPrefs.HasKey((DataHelper.PlayerDataKey.GAMEKEY) + PlayerController.Instance.GetUsername()))
            {
				string numbers = PlayerPrefs.GetString((DataHelper.PlayerDataKey.GAMEKEY) + PlayerController.Instance.GetUsername());
                numbers = numbers + "," + DataController.GameNumber;
				PlayerPrefs.SetString((DataHelper.PlayerDataKey.GAMEKEY) + PlayerController.Instance.GetUsername(), numbers);
				Debug.Log("games in player prefs = " + PlayerPrefs.GetString((DataHelper.PlayerDataKey.GAMEKEY) + PlayerController.Instance.GetUsername()) + PlayerController.Instance.GetUsername());
            }
            else
            {
				PlayerPrefs.SetString(((DataHelper.PlayerDataKey.GAMEKEY) + PlayerController.Instance.GetUsername()), DataController.GameNumber.ToString());
				Debug.Log("games in player prefs = " + PlayerPrefs.GetString((DataHelper.PlayerDataKey.GAMEKEY) + PlayerController.Instance.GetUsername()));
            }
            ShowQuestion();
        }

        private void Update()
        {
            _timeRemaining -= Time.deltaTime;

            UpdateTimeRemainingDisplay();

            if (_timeRemaining <= 0 && _timeRemaining > -100)
            {
                _timeRemaining = -101;
                Debug.Log("endround");
                EndRound();
            }
        }

        #endregion unity

        #region questionpool

        private QuestionData[] GetQuestionPool()
        {
            if (DataController.TurnNumber == 0)
            {
                //this should not actually happen as CheckForOpenGames() should have set the turn number to at least 1
                // treat this as a brand new game
                _questionPool = QuestionController.GetAllQuestionsAllCategories();
                return _questionPool;
            }

            if (DataController.TurnNumber == 1)
            {
                //there is no open games, the user is the 'player' they will be starting a new game which will get an opponent later
                Debug.Log("there are no open games, user set to player, starting brand new game");
                _questionPool = GameLobbyController.QuestionsPoolFromCatagory;

                if (!string.IsNullOrEmpty(FirebaseController.Instance.Token))
                    FirebaseController.Instance.CreateNotification(FirebaseController.Instance.Token, "Are you ready?", "It's your turn!");

                return _questionPool;
            }
            if (DataController.TurnNumber == 2)
            {
                //there is an open game, the user will be the 'oppponent' they will be playing round 2 with a predetermined questionpool
                string roundDataJSON = DataController.OngoingGameData.askedQuestions;
                RoundData rd = JsonUtility.FromJson<RoundData>(roundDataJSON);
                _questionPool = rd.questions;

                if (!string.IsNullOrEmpty(FirebaseController.Instance.Token))
                    FirebaseController.Instance.CreateNotification(FirebaseController.Instance.Token, "Fingers crossed!", "You've got a new opponent!");

                return _questionPool;
            }

            if (DataController.TurnNumber == 3)
            {
                //Continuing a game. The opponent now gets to pick the catagory
                _questionPool = GameLobbyController.QuestionsPoolFromCatagory;

                if (!string.IsNullOrEmpty(FirebaseController.Instance.Token))
                    FirebaseController.Instance.CreateNotification(FirebaseController.Instance.Token, "Are you ready?", "It's your turn!");

                return _questionPool;
            }

            if (DataController.TurnNumber == 4)
            {
                //there is an open game, the user will be the 'oppponent' they will be playing round 2 with a predetermined questionpool
                string roundDataJSON = DataController.OngoingGameData.askedQuestions;
                RoundData rd = JsonUtility.FromJson<RoundData>(roundDataJSON);
                _questionPool = rd.questions;

                if (!string.IsNullOrEmpty(FirebaseController.Instance.Token))
                    FirebaseController.Instance.CreateNotification(FirebaseController.Instance.Token, "Fingers crossed!", "You've got a new opponent!");

                return _questionPool;
            }
            if (DataController.TurnNumber == 5)
            {
                //there is no open games, the user is the 'player' they will be starting a new game which will get an opponent later
                _questionPool = GameLobbyController.QuestionsPoolFromCatagory;
                Destroy(GameLobbyController);

                if (!string.IsNullOrEmpty(FirebaseController.Instance.Token))
                    FirebaseController.Instance.CreateNotification(FirebaseController.Instance.Token, "Are you ready?", "It's your turn!");

                return _questionPool;
            }

            if (DataController.TurnNumber == 6)
            {
                string roundDataJSON = DataController.OngoingGameData.askedQuestions;
                RoundData rd = JsonUtility.FromJson<RoundData>(roundDataJSON);
                _questionPool = rd.questions;

                if (!string.IsNullOrEmpty(FirebaseController.Instance.Token))
                    FirebaseController.Instance.CreateNotification(FirebaseController.Instance.Token, "Fingers crossed!", "You've got a new opponent!");

                return _questionPool;
            }
            else
            {
                Debug.Log("something went wrong");
                _questionPool = QuestionController.GetAllQuestionsAllCategories();
                return _questionPool;
            }
        }

        #endregion questionpool

        #region display question & answers

        public void ShowQuestion()
        {
            scoreDisplay.text = PlayerController.UserScore.ToString();
            clicked = false;
            RemoveAnswerButtons();
            PlayerController.AddToTotalQuestionsAnswered();

            currentQuestionData = null;

            //if all questions are asked
            if (_questionPool.Length <= 0)
            {
                if (DataController.TurnNumber == 0 | DataController.TurnNumber == 1)
                {
                    //the user is the player so if they have finished the question pool they have answered all the questions in the catagory.
                    Debug.Log("GameController : Show Questions(): Out of Questions");
                    Debug.Log("endround");
                    EndRound();
                }
                if (DataController.TurnNumber == 2)
                {
                    //the user is the oppenent so if they have finished the question pool they have answered all the questions asked of the player, go on to the remaining questions in the catagory

                    if (DataController.OngoingGameData.questionsLeftInCat.Length < 5)
                    {
                        Debug.Log("endround");
                        EndRound();
                    }

                    string roundDataJSON = DataController.OngoingGameData.questionsLeftInCat;
                    Debug.Log("out of asked questons, asking remaining questions: " + DataController.OngoingGameData.questionsLeftInCat);

                    DataController.OngoingGameData.questionsLeftInCat = "";
                    RoundData rd = JsonUtility.FromJson<RoundData>(roundDataJSON);
                    _questionPool = rd.questions;
                    ShowQuestion();
                }
            }
            else
            { //ask the question
                if (DataController.TurnNumber == 0 | DataController.TurnNumber == 1 | DataController.TurnNumber == 3 | DataController.TurnNumber == 5)
                {
                    //if user is player then quesitons should be asked in random order
                    int randomNumber = Random.Range(0, _questionPool.Length - 1); //gets random number between 0 and total number of questions
                    currentQuestionData = _questionPool[randomNumber];// Get the QuestionData for the current question
                    _questionPool = QuestionController.RemoveQuestion(_questionPool, randomNumber); //remove question from list
                    questionText.text = currentQuestionData.questionText;  // Update questionText with the correct text
                    QuestionController.AddAskedQuestionToAskedQuestions(currentQuestionData);//keep track of the questions we asked so we can repeat it for the oppoent player
                    _numberOfQuestionsAsked++;
                    ShowAnswers(currentQuestionData);
                    if (!string.IsNullOrEmpty(FirebaseController.Instance.Token))
                        FirebaseController.Instance.CreateNotification(FirebaseController.Instance.Token, "Are you ready?", "It's your turn!");
                    else
                        return;
                }
                if (DataController.TurnNumber == 2 | DataController.TurnNumber == 4 | DataController.TurnNumber == 6)
                {
                    //if user is oppoent questions should be asked in the same order they were asked of player
                    currentQuestionData = _questionPool[0];// Get the QuestionData for the current question
                    _questionPool = QuestionController.RemoveQuestion(_questionPool, 0); //remove question from list
                    questionText.text = currentQuestionData.questionText;  // Update questionText with the correct text
                    _numberOfQuestionsAsked++;
                    ShowAnswers(currentQuestionData);
                    if (!string.IsNullOrEmpty(FirebaseController.Instance.Token))
                        FirebaseController.Instance.CreateNotification(FirebaseController.Instance.Token, "Fingers crossed!", "Your opponent is taking their turn!");
                    else
                        return;
                }
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
                    PlayerController.AddToNumberCorrectAnswers();
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

        public void ReportQuestion()
        {
            FeedbackClick.Play();
            FeedbackAlert.Show("Report question");
            DownvoteButton();
        }

        public void LikeQuestion()
        {
            FeedbackClick.Play();
            FeedbackAlert.Show("Like question");
            UpvoteButton();
        }

        #endregion like & dislike buttons

        #region scoring specific

        public void Score(bool answer)
        {
            Debug.Log("GameController : Score(): score called bool = " + answer);

            if (answer)
                PlayerController.UserScore = PlayerController.UserScore + 10;

            if (!answer)
                PlayerController.UserScore = PlayerController.UserScore - 5;

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

            if (!string.IsNullOrEmpty(FirebaseController.Instance.Token))
                FirebaseController.Instance.CreateNotification(FirebaseController.Instance.Token, "That's a wrap, folks!", "Your game has ended!");

            SceneManager.LoadScene(BuildIndex.Result, LoadSceneMode.Single);

            Debug.Log("GameController : EndRound(): End of Round");
            Debug.Log(PlayerController.ScoreStatus);
        }

        public void SubmitToOngoingGamesDB()
        {
            SubmitGame submitGame;
            submitGame = FindObjectOfType<SubmitGame>();
            submitGame.SubmitGameToDB(QuestionController.GetRemainingQuestions(_questionPool));
        }

        public void UpvoteButton()
        {
            Debug.Log("****current question is: " + currentQuestionData.questionText);
            _upVote.Uvote(currentQuestionData);
        }

        public void DownvoteButton()
        {
            Debug.Log("****current question is: " + currentQuestionData.questionText);
            _downVote.Dvote(currentQuestionData);
        }

        #endregion navigation specific

        #endregion methods
    }
}