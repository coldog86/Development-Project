using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class MenuController : MonoBehaviour
    {
        [Header("Setting")]
        public int loginIndex = 1;
        public int gameIndex = 3;
        public int accountIndex = 4;
        public int leaderboardIndex = 5;
        public int settingIndex = 6;

       
        private FeedbackAlert _alert;
        private FeedbackClick _click;
        private FeedbackModal _modal;

        private void Start()
        {
            _alert = FindObjectOfType<FeedbackAlert>();
            _click = FindObjectOfType<FeedbackClick>();
            _modal = FindObjectOfType<FeedbackModal>();
        }

        // TASK : TO BE COMPLETED
        public void StartGame()
        {
        }

        // TASK : TO BE COMPLETED
        public void ContinueGame()
        {
        }

        public void OpenAccount()
        {
            _click.Play();
            SceneManager.LoadScene(accountIndex, LoadSceneMode.Single);
        }

        public void OpenLeaderboard()
        {
            _click.Play();
            SceneManager.LoadScene(leaderboardIndex, LoadSceneMode.Single);
        }

        public void OpenSetting()
        {
            _click.Play();
            SceneManager.LoadScene(settingIndex, LoadSceneMode.Single);
        }

        public void Logout()
        {
            _modal.Show(false, "Are you sure?", "Are you sure you want to logout?", null, "No", "Yes");
            _modal.positiveButton.onClick.AddListener(OpenLogin);
        }

        private void OpenLogin()
        {
            _click.Play();
            SceneManager.LoadScene(loginIndex, LoadSceneMode.Single);
        }

        public void Quit()
        {
            _click.Play();

            _modal.Show(false, "Are you sure?", "Are you sure you want to quit?", null, "No", "Yes");
            _modal.positiveButton.onClick.AddListener(Quit);

            #if PLATFORM_ANDROID 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _modal.Show(false, "Are you sure?", "Are you sure you want to quit?", null, "No", "Yes");
                _modal.positiveButton.onClick.AddListener(Quit);
            }  
            #endif

            // Debug purposes only
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}