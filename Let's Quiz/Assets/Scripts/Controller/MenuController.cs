using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller
{
    public class MenuController : MonoBehaviour
    {
        [Header("Setting")]
        public int gameSceneIndex = 2;

        public void StartGame()
        {
            SceneManager.LoadScene(gameSceneIndex, LoadSceneMode.Single);
        }

        public void OpenHighscore()
        {
            Debug.Log("MenuController : OpenHighscore()");
        }

        public void OpenOptions()
        {
            Debug.Log("MenuController : OpenOptions()");
        }

        public void QuitGame()
        {
            Application.Quit();

            // Android Back Button Action
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            // Debug purposes only
            UnityEditor.EditorApplication.isPlaying = false;
        }
                
    }
}