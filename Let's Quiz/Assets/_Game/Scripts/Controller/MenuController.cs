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
       
        private FeedbackClick _click;

        private void Start()
        {
            _click = FindObjectOfType<FeedbackClick>();

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
            CloseNavDrawer();
            _click.Play();
            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to log out?", "Log out", "Cancel", OpenLogin, FeedbackTwoButtonModal.Hide);
        }

        private void OpenLogin()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndexHelper.Login, LoadSceneMode.Single);
        }

        public void Quit()
        {
            CloseNavDrawer();
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
    }
}