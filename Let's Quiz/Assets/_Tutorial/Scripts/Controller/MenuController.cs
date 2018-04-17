using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Tutorial
{
    public class MenuController : MonoBehaviour
    {
        [Header("Setting")]
        public int gameSceneIndex = 2;
        public int optionsSceneIndex = 3;

        public void StartGame()
        {
            SceneManager.LoadScene(gameSceneIndex, LoadSceneMode.Single);
        }

        public void OpenOptions()
        {
            SceneManager.LoadScene(optionsSceneIndex, LoadSceneMode.Single);
        }

        public void QuitGame()
        {
            Application.Quit();

            // Android Back Button Action
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            // Debug purposes only
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
