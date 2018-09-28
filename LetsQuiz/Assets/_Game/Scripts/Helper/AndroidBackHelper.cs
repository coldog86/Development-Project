using UnityEngine;
using UnityEngine.SceneManagement;

namespace _LetsQuiz
{
    public class AndroidBackHelper : MonoBehaviour
    {
        #region variables

        private GameController _gameController;
        private bool _showingModal = false;

        #endregion variables

        #region methods

        public void Update()
        {
            if (_showingModal)
                return;

            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case BuildIndex.Splash:
                case BuildIndex.Login:
                case BuildIndex.Menu:

                    if (Input.GetKey(KeyCode.Escape))
                    {
                        if (!_showingModal)
                        {
                            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", Application.Quit, Hide);
                            _showingModal = true;
                        }
                    }

                    break;

                case BuildIndex.GameLobby:
                case BuildIndex.Result:
                case BuildIndex.Account:
                case BuildIndex.Leaderboard:
                case BuildIndex.SubmitQuestion:
                case BuildIndex.Settings:

                    if (Input.GetKey(KeyCode.Escape))
                        SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);

                    break;

                case BuildIndex.Game:

                    if (Input.GetKey(KeyCode.Escape))
                    {
                        _gameController = FindObjectOfType<GameController>();

                        if (!_showingModal)
                        {
                            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to forfeit this game?", "Yes", "No", ForfeitGame, Hide);
                            _showingModal = true;
                        }
                    }

                    break;
            }
        }

        private void ForfeitGame()
        {
            _showingModal = false;
            _gameController.EndRound();
        }

        private void Hide()
        {
            _showingModal = false;
            FeedbackTwoButtonModal.Hide();
        }

        #endregion methods
    }
}