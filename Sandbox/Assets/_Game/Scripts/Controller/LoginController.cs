using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Controller
{
    public class LoginController : MonoBehaviour
    {
        [Header("Setting")]
        public int menuIndex = 2;

        public void Login()
        {
            SceneManager.LoadScene(menuIndex, LoadSceneMode.Single);
        }

        public void LoginGoogle()
        {
            SceneManager.LoadScene(menuIndex, LoadSceneMode.Single);
        }

        public void LoginFacebook()
        {
            SceneManager.LoadScene(menuIndex, LoadSceneMode.Single);
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