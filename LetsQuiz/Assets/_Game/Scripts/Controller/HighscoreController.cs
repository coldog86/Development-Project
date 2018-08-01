﻿using UnityEngine;

namespace _LetsQuiz
{
    public class HighscoreController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        private PlayerController _playerController;
        private string _highScoreData;

        #endregion variables

        #region methods

        public void Load()
        {
            Debug.Log("[HighscoreController] Load()");
            _playerController = FindObjectOfType<PlayerController>();
            _highScoreData = _playerController.highScoreJSON;
        }

        public HighScoresContainer extractHighScores()
        {
            HighScoresContainer allHighScorers = JsonUtility.FromJson<HighScoresContainer>(_highScoreData);
            return allHighScorers;
        }

        #endregion methods
    }
}