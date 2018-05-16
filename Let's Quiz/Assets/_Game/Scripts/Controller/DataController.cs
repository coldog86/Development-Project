using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace _LetsQuiz
{
    public class DataController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        private GetAllQuestions _questionDownload;
        private PlayerController _playerController;
        private SettingsController _settingsController;
        private QuestionController _questionController;

        [Header("Player")]
        private Player _player;
        private string _playerString = "";


        [Header("Connection")]
        private float _connectionTimeLimit = 1000000.0f;
        private float _connectionTimer = 0.0f;

        [Header("Validation Tests")]
        private string _username = "u";
        private string _password = "p";
        private int _status = -2;

        #endregion

        #region properties

        public bool serverConnected { get; set; }

        public string allQuestionJSON { get; set; }

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            _settingsController = GetComponent<SettingsController>();
            _settingsController.Load();

            _playerController = GetComponent<PlayerController>();
            _playerController.Load();

            _questionDownload = FindObjectOfType<GetAllQuestions>();
            StartCoroutine(_questionDownload.PullAllQuestionsFromServer());

            _questionController = GetComponent<QuestionController>();
            _questionController.Load();

            // retrive player username and password from PlayerPrefs if they have an id
            if (PlayerPrefs.HasKey(_playerController.idKey))
            {
                _status = _playerController.GetPlayerType();
                _username = _playerController.GetUsername();
                _password = _playerController.GetPassword();
            }
        }

        private void Quit()
        {
            Application.Quit();

            // NOTE : debug purposes only
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        #endregion

        #region server specific

        public void Init()
        {
            if (serverConnected)
            {
                // check if username and password are stored in PlayerPrefs
                // if it is login, otherwise load login scene
                if (_username != "u" && _password != "p" && _status == PlayerStatus.LoggedIn)
                    StartCoroutine(Login(_username, _password));
                else
                    SceneManager.LoadScene(BuildIndex.Login);
            }
            // prompt user if they wish to retry
            else
                DisplayErrorModal("Error connecting to the server.");
        }

        private IEnumerator Login(string username, string password)
        {
            WWWForm form = new WWWForm();

            form.AddField("usernamePost", username);
            form.AddField("passwordPost", password);

            WWW loginRequest = new WWW(ServerHelper.Host + ServerHelper.Login, form);

            while (!loginRequest.isDone)
            {
                _connectionTimer += Time.deltaTime;

                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("DataController : Login() : " + loginRequest.error);
                    yield return null;
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || loginRequest.error != null)
                {
                    FeedbackAlert.Show("Server error.");
                    Debug.LogError("DataController : Login() : " + loginRequest.error);
                    yield return null;
                }    
            }

            if (loginRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.Log("DataController : Login() : " + loginRequest.error);
                yield return null;
            }

            if (loginRequest.isDone)
            {
                // check that the login request returned something
                if (!String.IsNullOrEmpty(loginRequest.text))
                {
                    _playerString = loginRequest.text;
                    Debug.Log(_playerString);

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
                            _playerController.Save(_player.ID, _player.username, _player.email, _player.password, _player.DOB, _player.questionsSubmitted, 
                                _player.numQuestionsSubmitted, _player.numGamesPlayed, _player.highestScore, 
                                _player.numCorrectAnswers, _player.totalQuestionsAnswered);

                        FeedbackAlert.Show("Welcome back " + _username);
                        SceneManager.LoadScene(BuildIndex.Menu);
                        yield return loginRequest;
                    }
                }
            }
        }

        private void RetryPullData()
        {
            FeedbackAlert.Show("Retrying connection...", 1.0f);
            StartCoroutine(_questionDownload.PullAllQuestionsFromServer());
        }

        #endregion

        #region feedback specific

        // positive action - retry question download
        // negative action - quit application
        private void DisplayErrorModal(string message)
        {
            FeedbackTwoButtonModal.Show("Error!", message + "\nDo you wish to retry?", "Yes", "No", RetryPullData, Quit);
        }

        #endregion

        #endregion
    }
}
