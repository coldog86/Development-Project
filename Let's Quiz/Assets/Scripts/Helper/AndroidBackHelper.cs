using UnityEngine;
using UnityEngine.SceneManagement;

namespace _LetsQuiz
{
    public class AndroidBackHelper : MonoBehaviour
    {
        #region variables

        GameController _gameController;

        #endregion

        #region methods

        #if PLATFORM_ANDROID
        public void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == BuildIndex.Splash ||
                SceneManager.GetActiveScene().buildIndex == BuildIndex.Login ||
                SceneManager.GetActiveScene().buildIndex == BuildIndex.Menu)
            {
                if (Input.GetKey(KeyCode.Escape))
                    FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", Application.Quit, FeedbackTwoButtonModal.Hide);
                
            }
            else if (SceneManager.GetActiveScene().buildIndex == BuildIndex.Account ||
                     SceneManager.GetActiveScene().buildIndex == BuildIndex.Leaderboard ||
                     SceneManager.GetActiveScene().buildIndex == BuildIndex.SubmitQuestion ||
                     SceneManager.GetActiveScene().buildIndex == BuildIndex.Settings)
            {
                if (Input.GetKey(KeyCode.Escape))
                    SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
            }
            else if (SceneManager.GetActiveScene().buildIndex == BuildIndex.Game)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    _gameController = FindObjectOfType<GameController>();
                    FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to forfeit this game?", "Yes", "No", ForfeitGame, FeedbackTwoButtonModal.Hide);
                }    
            }
        }
        #endif

        private void ForfeitGame()
        {
            _gameController.EndRound();
        }

        #endregion
    }
}

