using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace _LetsQuiz
{
    public class DataController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        private GetAllQuestions _questionDownload;
        private PlayerController _playerController;
        private SettingsController _settingsController;
        private LoadHelper _loadHelper;
		private QuestionController _questionController;

        [Header("Player")]
        private Player player;
        private string playerString = "";


        [Header("Connection")]
        private float _connectionTimeLimit = 1000000.0f;
        private float _connectionTimer = 0.0f;

        [Header("Validation Tests")]
        private string _username = "u";
        private string _password = "p";

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

            _loadHelper = GetComponent<LoadHelper>();

            _settingsController = GetComponent<SettingsController>();
            _settingsController.Load();

            _playerController = GetComponent<PlayerController>();
            _playerController.Load();

            _questionDownload = FindObjectOfType<GetAllQuestions>();
           StartCoroutine(_questionDownload.PullAllQuestionsFromServer());

			_questionController = GetComponent<QuestionController>();


            if (PlayerPrefs.HasKey(_playerController.idKey))
            {
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
                if (_username != "u" && _password != "p")
                    StartCoroutine(Login(_username, _password));
                else
                    _loadHelper.Load(BuildIndex.Login);
            }
            else
                DisplayErrorModal("There was an error connection to the server.");
        }

        private void RetryPullData()
        {
            FeedbackAlert.Show("Retrying connection...", 1.0f);
            StartCoroutine(_questionDownload.PullAllQuestionsFromServer());
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
                    Debug.LogError("Server time out.");
                    Debug.LogError(loginRequest.error);
                    yield return null;
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || loginRequest.error != null)
                {
                    FeedbackAlert.Show("Server error.");
                    Debug.LogError(loginRequest.error);
                    yield return null;
                }    
            }

            if (loginRequest.isDone)
            {
                if (!loginRequest.text.Contains("ID"))
                {
                    _loadHelper.Load(BuildIndex.Login);
                    yield return null;
                }
                else
                {
                    playerString = loginRequest.text;
                    player = PlayerJsonHelper.LoadPlayerFromServer(playerString);

                    if (player != null)
                        _playerController.Save(player.ID, player.username, player.email, player.password, player.DOB, player.questionsSubmitted, 
                            player.numQuestionsSubmitted, player.numGamesPlayed, player.highestScore, 
                            player.numCorrectAnswers, player.totalQuestionsAnswered);
                    
                    FeedbackAlert.Show("Welcome back " + _username);
                    _loadHelper.Load(BuildIndex.Menu);
                    yield return loginRequest;
                }
            }
        }

        #endregion

        #region feedback specific

        private void DisplayErrorModal(string message)
        {
            FeedbackTwoButtonModal.Show("Error!", message + "\nDo you wish to retry?", "Yes", "No", RetryPullData, Quit);
        }

        #endregion

        #endregion
    }
}
