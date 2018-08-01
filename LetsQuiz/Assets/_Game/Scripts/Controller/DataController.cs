using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _LetsQuiz
{
    public class DataController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        private GetAllQuestions _questionDownload;
        private GetHighScores _highscoreDownload;
        private GetPlayerQuestionSubmissions _questandSub;
        private PlayerController _playerController;
        private SettingsController _settingsController;
        private QuestionController _questionController;
        private HighscoreController _highScoreController;

        [Header("Player")]
        private Player _player;
        private string _playerString = "";

        [Header("Connection")]
        private const float _connectionTimeLimit = 1000000.0f;
        private float _connectionTimer = 0.0f;

        [Header("Validation Tests")]
        private string _username = "u";
        private string _password = "p";
        private int _status = -2;

        public OngoingGamesData ongoingGameData { get; set; }

        public int turnNumber { get; set; }

        public int gameNumber { get; set; }

        #endregion variables

        #region properties

        public bool serverConnected { get; set; }

        public string allQuestionJSON { get; set; }

        public string allHighScoreJSON { get; set; }

        #endregion properties

        #region methods

        #region unity

        private void Start()
        {
            //PlayerPrefs.DeleteKey("col"); PlayerPrefs.DeleteKey("col2");
            DontDestroyOnLoad(gameObject);
            turnNumber = 0;
            _settingsController = GetComponent<SettingsController>();
            _settingsController.Load();

            _playerController = GetComponent<PlayerController>();
            _playerController.Load();

            _questionDownload = FindObjectOfType<GetAllQuestions>();
            StartCoroutine(_questionDownload.PullAllQuestionsFromServer());

            _highscoreDownload = FindObjectOfType<GetHighScores>();
            StartCoroutine(_highscoreDownload.PullAllHighScoresFromServer());

            //add in for the Submitted Questions Highscore table.
            _questandSub = GetComponent<GetPlayerQuestionSubmissions>();
            StartCoroutine(_questandSub.PullQuestionSubmitters());

            _questionController = GetComponent<QuestionController>();
            _questionController.Load();

            _highScoreController = GetComponent<HighscoreController>();
            _highScoreController.Load();

            // retrive player username and password from PlayerPrefs if they have an id
            if (PlayerPrefs.HasKey(_playerController.idKey))
            {//TODO is any of these ever used?
                _status = _playerController.GetPlayerType();
                _username = _playerController.GetUsername();
                _password = _playerController.GetPassword();
            }
        }

        #endregion unity

        #region server specific

        public void Init()
        {
            if (serverConnected)
            {
                // check if username and password are stored in PlayerPrefs
                // if it is login, otherwise load login scene
                if (_username != "u" && _password != "p" && (_status == PlayerStatus.LoggedIn || _status == PlayerStatus.Guest))
                    StartCoroutine(Login(_username, _password));
                else
                    SceneManager.LoadScene(BuildIndex.Login);
            }
            // prompt user if they wish to retry
            else
                DisplayRetryModal("Error connecting to the server.");
        }

        private IEnumerator Login(string username, string password)
        {
            WWWForm form = new WWWForm();

            form.AddField("usernamePost", username);
            form.AddField("passwordPost", password);

            WWW loginRequest = new WWW(ServerHelper.Host + ServerHelper.Login, form);

            _connectionTimer += Time.deltaTime;

            while (!loginRequest.isDone)
            {
                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("[DataController] Login() :  Server time out : " + loginRequest.error);
                    yield return null;
                }
                else if (loginRequest.error != null)
                {
                    FeedbackAlert.Show("Connection error. Please try again.");
                    Debug.LogError("[DataController] Login() : Server error " + loginRequest.error);
                    yield return null;
                }
                // extra check just to ensure a stream error doesn't come up
                else if (_connectionTimer > _connectionTimeLimit && loginRequest.error != null)
                {
                    FeedbackAlert.Show("Server error.");
                    Debug.LogError("[DataController] Login() : Server error : " + loginRequest.error);
                    yield return null;
                }
            }

            if (loginRequest.isDone && loginRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.LogError("[DataController] Login() : Server error " + loginRequest.error);
                yield return null;
            }

            if (loginRequest.isDone)
            {
                // check that the login request returned something
                if (!String.IsNullOrEmpty(loginRequest.text))
                {
                    _playerString = loginRequest.text;
                    //Debug.Log(_playerString);
                    _player = new Player();

                    //TODO Chanes can you look at the whole player and playercontroller and get rid of what we don't need please?
                    _player = JsonUtility.FromJson<Player>(_playerString);
                    //Debug.Log(_player.ID);

                    // if the retrieved login text doesn't have "ID" load login scene
                    if (!_playerString.Contains("ID"))
                    {
                        SceneManager.LoadScene(BuildIndex.Login);
                        yield return null;
                    }
                    // otherwise save the player information to PlayerPrefs and load menu scene
                    else
                    {
                        _player = PlayerJsonHelper.LoadPlayerFromServer(_playerString);

                        if (_player != null)
                        {
                            _playerController.Save(_player.ID, _player.username, _player.email, _player.password, _player.DOB, _player.questionsSubmitted,
                                _player.numQuestionsSubmitted, _player.numGamesPlayed, _player.totalPointsScore,
                                _player.TotalCorrectAnswers, _player.totalQuestionsAnswered);

                            FeedbackAlert.Show("Welcome back " + _username);
                        }
                        SceneManager.LoadScene(BuildIndex.Menu);
                        yield return loginRequest;
                    }
                }
            }
        }

        private void RetryPullData()
        {
            Debug.Log("[DataController] RetryPullData()");
            FeedbackAlert.Show("Retrying connection...", 1.0f);
            StartCoroutine(_questionDownload.PullAllQuestionsFromServer());
        }

        #endregion server specific

        #region feedback specific

        // positive action - retry question download
        // negative action - quit application
        private void DisplayRetryModal(string message)
        {
            FeedbackTwoButtonModal.Show("Error!", message + "\nDo you wish to retry?", "Yes", "No", RetryPullData, Application.Quit);
        }

        public int getOverAllScore()
        {
            if (_playerController.userScore > ongoingGameData.playerScore)
                ongoingGameData.overAllScore = -1; //opponent won the round
            if (_playerController.userScore < ongoingGameData.playerScore)
                ongoingGameData.overAllScore = +1; //player won the round

            return ongoingGameData.overAllScore;
        }

        #endregion feedback specific

        #endregion methods
    }
}