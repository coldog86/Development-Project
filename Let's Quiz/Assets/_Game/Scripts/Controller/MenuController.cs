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
       
        private Text _usernameText;
        private FeedbackClick _click;
        private SettingsController _settingsController;
        private LoadHelper _loadHelper;

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            _usernameText = GameObject.FindGameObjectWithTag("Username_Text").GetComponent<Text>();
            _settingsController = FindObjectOfType<SettingsController>();

            if (_settingsController.GetPlayerType() == PlayerStatus.Guest)
                _usernameText.text = "Guest";
            else
                _usernameText.text = "Test";
        }

        private void Start()
        {
           
            navigationDrawer.SetActive(false);
            _click = FindObjectOfType<FeedbackClick>();
            _loadHelper = FindObjectOfType<LoadHelper>();
            Destroy(_loadHelper);
        }

        #endregion

        #region game specific

        // TASK : TO BE COMPLETED
        public void StartGame()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Game, LoadSceneMode.Single);
        }

        // NOTE : PLACEHOLDER
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

        // NOTE : DEBUG PURPOSES ONLY
        // TASK : TO BE COMPLETED
        public void Logout()
        {
            _click.Play();
            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to log out?", "Log out", "Cancel", OpenLogin, FeedbackTwoButtonModal.Hide);
        }

        private void OpenLogin()
        {
            _click.Play();
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

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

        }

        #endregion

        #endregion
    }
}