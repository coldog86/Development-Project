using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class MenuController : MonoBehaviour
    {
        [Header("Component")]
        public GameObject navBar;
       
        // private FeedbackAlert _alert;
        private FeedbackClick _click;
        private FeedbackModal _modal;

        private void Start()
        {
            // _alert = FindObjectOfType<FeedbackAlert>();
            _click = FindObjectOfType<FeedbackClick>();
            _modal = FindObjectOfType<FeedbackModal>();

            navBar.SetActive(false);
        }

        // TASK : TO BE COMPLETED
        public void StartGame()
        {
        }

        // TASK : TO BE COMPLETED
        public void ContinueGame()
        {
        }

        public void OpenNavDrawer()
        {
            _click.Play();
            navBar.SetActive(true);
        }

        public void CloseNavDrawer()
        {
            _click.Play();
            navBar.SetActive(false);
        }

        public void OpenAccount()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndexHelper.Account, LoadSceneMode.Single);
        }

        public void OpenLeaderboard()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndexHelper.Leaderboard, LoadSceneMode.Single);
        }

        public void OpenSetting()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndexHelper.Settings, LoadSceneMode.Single);
        }

        // NOTE : DEBUG PURPOSES ONLY
        // TASK : TO BE COMPLETED
        public void Logout()
        {
            _click.Play();
            _modal.Show(false, "Are you sure?", "Are you sure you want to logout?", null, "No", "Yes");
            _modal.positiveButton.onClick.AddListener(OpenLogin);
        }

        private void OpenLogin()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndexHelper.Login, LoadSceneMode.Single);
        }

        public void Quit()
        {
            _click.Play();
            _modal.Show(false, "Are you sure?", "Are you sure you want to quit?", null, "No", "Yes");
            _modal.positiveButton.onClick.AddListener(QuitGame);
        }

        private void QuitGame()
        {
            Application.Quit();

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

        }
    }
}