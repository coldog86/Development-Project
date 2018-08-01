﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class MenuController : MonoBehaviour
    {
        #region variables

        [Header("Component")]
        // public GameObject navigationDrawer;
        public Button accountButton;
        public Button leaderboardButton;
        public Button submitQuestionButton;
       
        private Text _username;
        private FeedbackMusic _music;
        private PlayerController _playerController;
        private GetAllQuestions _questionDownload;

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            _username = GameObject.FindGameObjectWithTag("Username_Text").GetComponent<Text>();
            _playerController = FindObjectOfType<PlayerController>();

            var playerType = _playerController.GetPlayerType();

            if (PlayerPrefs.HasKey(_playerController.usernameKey) && (playerType == PlayerStatus.LoggedIn || playerType == PlayerStatus.Guest))
                _username.text = _playerController.GetUsername();

            if (playerType == PlayerStatus.Guest)
            {
                accountButton.gameObject.SetActive(false);
                leaderboardButton.gameObject.SetActive(false);
                submitQuestionButton.gameObject.SetActive(false);
            }  
        }
        //TODO what is this for, I have seen a few private start() methods, when are they called, do they behave like a public Start()?
        // NOTE : does the same as public start() - just better programming pratise
        private void Start()
        {
            //navigationDrawer.SetActive(false);

            _music = FindObjectOfType<FeedbackMusic>();

            _questionDownload = FindObjectOfType<GetAllQuestions>();

            Destroy(_questionDownload);

        }

        #endregion

        #region game specific

        public void StartGame()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Game, LoadSceneMode.Single);
        }

        public void GoToGameLobby()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.GameLobby, LoadSceneMode.Single);
        }

        #endregion

        #region navigation specific

        public void OpenAccount()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Account, LoadSceneMode.Single);
        }

        public void OpenLeaderboard()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Leaderboard, LoadSceneMode.Single);
        }

        public void OpenSubmitQuestion()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.SubmitQuestion, LoadSceneMode.Single);
        }

        public void OpenSetting()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Settings, LoadSceneMode.Single);
        }

        public void Logout()
        {
            FeedbackClick.Play();
            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to log out?", "Log out", "Cancel", OpenLogin, FeedbackTwoButtonModal.Hide);
        }

        private void OpenLogin()
        {
            FeedbackClick.Play();
            _playerController.SetPlayerType(PlayerStatus.LoggedOut);
            SceneManager.LoadScene(BuildIndex.Login, LoadSceneMode.Single);
        }

        public void Quit()
        {
            FeedbackClick.Play();
            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", Application.Quit, FeedbackTwoButtonModal.Hide);
        }

        #endregion

        #endregion
    }
}