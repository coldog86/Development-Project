using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Configuration;
using UnityEditor.MemoryProfiler;
using System;

namespace _LetsQuiz
{
    public class DataController : MonoBehaviour
    {
        #region variables

        [Header("Server")]
        private string _hostUrl = "www.41melquizgame.xyz/LQ/";
        private string _connectionFile = "loginExistingUserWithEmail.php";

        [Header("Connection")]
        private float _connectionTimeLimit = 1000000.0f;
        private float _connectionTimer = 0.0f;

        [SerializeField]
        private string _username = "u";
        [SerializeField]
        private string _password = "p";

        private GetAllQuestions _questionDownload;
        private PlayerController _playerController;
        private SettingsController _settingsController;
        private LoadHelper _loadHelper;

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
            _settingsController.LoadPlayerSettings();

            _playerController = GetComponent<PlayerController>();
            _playerController.LoadPlayer();

            if (PlayerPrefs.HasKey(_playerController.usernameKey) && PlayerPrefs.HasKey(_playerController.passwordKey))
            {
                _username = _playerController.GetUsername();
                _password = _playerController.GetPassword();
            }

            // StartCoroutine(ConnectToServer());

            _questionDownload = FindObjectOfType<GetAllQuestions>();
            StartCoroutine(_questionDownload.PullAllQuestionsFromServer());
        }

        private void Quit()
        {
            Application.Quit();

            // NOTE : DEBUG PURPOSES ONLY
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
                {
                    FeedbackAlert.Show("Welcome back " + _username);
                    StartCoroutine(Login(_username, _password));
                }
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

            WWW loginRequest = new WWW(_hostUrl + _connectionFile, form);

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

        #region obsolete - only keeping for historical purposes

        [Obsolete("ConnectToServer is deprecated, please use _questionDownload.PullAllQuestionsFromServer instead.")]
        private IEnumerator ConnectToServer()
        {
            WWW open = new WWW(_hostUrl + _connectionFile);

            while (!open.isDone)
            {
                _connectionTimer += Time.deltaTime;
                if (_connectionTimer > _connectionTimeLimit)
                    ShowModal("Timeout Error.");
                yield return null;
            }

            if (open.error != null)
            {
                ShowModal("Connection Error.");
                yield return null;
            }
            else
            {
                yield return open;

                var playerType = _playerController.GetPlayerType();

                if (playerType != PlayerStatus.New)
                    _loadHelper.Load(BuildIndex.Menu);
                else
                    _loadHelper.Load(BuildIndex.Login);
            }
        }

        [Obsolete("RetryConnection is deprecated, please use RetryPullData instead.")]
        private void RetryConnection()
        {
            FeedbackAlert.Show("Retrying connection...", 1.0f);
            StartCoroutine(ConnectToServer());
        }

        [Obsolete("ShowModal is deprecated, please use DisplayErorModal instead.")]
        private void ShowModal(string message)
        {
            FeedbackTwoButtonModal.Show("Error!", message + "\nDo you wish to retry?", "Yes", "No", RetryConnection, Quit);
        }

        #endregion

        #endregion
    }
}