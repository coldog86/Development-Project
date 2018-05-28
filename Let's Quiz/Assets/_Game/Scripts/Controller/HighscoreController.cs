using UnityEngine;

namespace _LetsQuiz
{
    public class HighscoreController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        private PlayerController _playerController;
        private string _highScoreData;

        #endregion

        #region methods

        public void Load()
        {
            Debug.LogError("HighscoreController : Load()");
            _playerController = FindObjectOfType<PlayerController>();
            _highScoreData = _playerController.highScoreJSON;
        }

        public HighScoresContainer extractHighScores()
        {
            HighScoresContainer allHighScorers = JsonUtility.FromJson<HighScoresContainer>(_highScoreData);
            return allHighScorers;
        }

        #endregion
    }
}