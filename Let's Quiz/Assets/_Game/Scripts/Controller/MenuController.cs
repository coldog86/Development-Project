using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class MenuController : MonoBehaviour
    {
        #region variables

        [Header("Component")]
        public GameObject navigationDrawer;
		public Button submitQuestionButton;
       
        private Text _usernameText;
        private FeedbackClick _click;
        private FeedbackMusic _music;
        private PlayerController _playerController;
        private GetAllQuestions _questionDownload;

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            _usernameText = GameObject.FindGameObjectWithTag("Username_Text").GetComponent<Text>();
            _playerController = FindObjectOfType<PlayerController>();

            var playerType = _playerController.GetPlayerType();

            if (PlayerPrefs.HasKey(_playerController.usernameKey) && playerType == PlayerStatus.LoggedIn)
                _usernameText.text = _playerController.GetUsername();

			if (_playerController.GetPlayerType () == -1)
				submitQuestionButton.gameObject.SetActive (false);
                
        }

        private void Start()
        {
            //navigationDrawer.SetActive(false);

            _click = FindObjectOfType<FeedbackClick>();
            _music = FindObjectOfType<FeedbackMusic>();

            _questionDownload = FindObjectOfType<GetAllQuestions>();

            Destroy(_questionDownload);
        }

        private void Update()
        {
            // NOTE : android platform only
            #if PLATFORM_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape))
                FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", QuitGame, FeedbackTwoButtonModal.Hide);
            #endif
        }

        #endregion

        #region game specific

        // TASK : TO BE COMPLETED
        public void StartGame()
        {
            _click.Play();
            _music.PlayGameMusic();
            SceneManager.LoadScene(BuildIndex.Game, LoadSceneMode.Single);
        }

        // NOTE : TO BE COMPLETED
        public void LoadActiveGames()
        {
        }

        // TASK : TO BE COMPLETED
        public void ContinueGame(int gameId)
        {
        }

        #endregion

        #region navigation drawer specific

        public void OpenNavDrawer()
        {
            _click.Play();
            navigationDrawer.SetActive(true);
        }

        public void CloseNavDrawer()
        {
            _click.Play();
            navigationDrawer.SetActive(false);
        }

        #endregion

        #region navigation specific

        public void OpenAccount()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Account, LoadSceneMode.Single);
        }

        public void OpenLeaderboard()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Leaderboard, LoadSceneMode.Single);
        }

        public void OpenSubmitQuestion()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.SubmitQuestion, LoadSceneMode.Single);
        }

        public void OpenSetting()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Settings, LoadSceneMode.Single);
        }

        public void Logout()
        {
            _click.Play();
            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to log out?", "Log out", "Cancel", OpenLogin, FeedbackTwoButtonModal.Hide);
        }

        private void OpenLogin()
        {
            _click.Play();
            _playerController.SetPlayerType(PlayerStatus.LoggedOut);
            SceneManager.LoadScene(BuildIndex.Login, LoadSceneMode.Single);
        }

        public void Quit()
        {
            _click.Play();
            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", QuitGame, FeedbackTwoButtonModal.Hide);
        }

        private void QuitGame()
        {
            Application.Quit();

            // NOTE : debug purposes only
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        #endregion

        #endregion
    }
}