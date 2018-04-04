using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Controller
{
    public class MenuController : MonoBehaviour
    {
        [Header("Setting")]
        public int loginIndex = 1;
        public int gameIndex = 3;
        public int accountIndex = 4;
        public int leaderboardIndex = 5;
        public int achievementIndex = 6;
        public int settingsIndex = 7;

        public void StartGame()
        {
            Debug.Log("Start Game");
        }

        public void OpenAccount()
        {
            Debug.Log("Open Account");
        }

        public void OpenLeaderboard()
        {
            Debug.Log("Open Leaderboard");
        }

        public void OpenAchievement()
        {
            Debug.Log("Open Achievement");
        }

        public void OpenSettings()
        {
            Debug.Log("Open Settings");
        }

        public void Logout()
        {
            SceneManager.LoadScene(loginIndex, LoadSceneMode.Single);
        }

        public void QuitGame()
        {
            Application.Quit();

            // Android Back Button Action
            #if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
            #endif

            // Debug purposes only
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}

