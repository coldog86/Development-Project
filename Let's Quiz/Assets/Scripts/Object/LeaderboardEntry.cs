using UnityEngine;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class LeaderboardEntry : MonoBehaviour
    {
        public Text usernameText;
        public Text scoreText;

        private GameController _gameController;
        private RectTransform transform;

        void Start()
        {
            _gameController = FindObjectOfType<GameController>();
            transform = GetComponent<RectTransform>();

            // in case scale goes a bit funny
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public void SetUp(string username, string score)
        {
            usernameText.text = username;
            scoreText.text = score;
            Debug.Log(usernameText.text);
        }
    }
}