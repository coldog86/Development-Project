using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class GameLobbyController : MonoBehaviour
    {
        private CheckForOpenGames _CheckForOpenGames;
        private OngoingGamesData _DataForExistingGame;
        private MenuController _MenuController;
        private PlayerController _playerController;
        private CheckForPlayerExistingGames _checkForPlayerExistingGames;

        private int[] gameNumbers;

        public Button ongoingGame1;
        public Button ongoingGame2;
        public Button ongoingGame3;
        public Button ongoingGame4;

        private FeedbackClick _click;

        // Use this for initialization
        private void Start()
        {
            // reference to play annoying clicky sound
            _click = FindObjectOfType<FeedbackClick>();

            _CheckForOpenGames = FindObjectOfType<CheckForOpenGames>();
            _MenuController = FindObjectOfType<MenuController>();
            _checkForPlayerExistingGames = FindObjectOfType<CheckForPlayerExistingGames>();
            _playerController = FindObjectOfType<PlayerController>();

            //PlayerPrefs.SetString(_playerController.GetUsername(), "55194,26117,37969,39617");

            Debug.Log("here we go");
            if (PlayerPrefs.HasKey(_playerController.GetUsername()))
            {
                //Debug.Log(PlayerPrefs.GetString(_playerController.ongoingGamesKey));
                Debug.Log(PlayerPrefs.GetString(_playerController.GetUsername()));
                _checkForPlayerExistingGames.GetPlayersOpenGames();
            }
            else
                Debug.Log("player has no ongoing games");
        }

        public void StartOpenGame(OngoingGamesData _ongoingGameData)
        {
            Debug.Log(_ongoingGameData.askedQuestions);
        }

        public void StartNewGame()
        {
            Debug.Log("checking for open games");
            _CheckForOpenGames.CheckForGamesNeedingOpponents();
        }

        public void BackToMenu()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }
    }
}