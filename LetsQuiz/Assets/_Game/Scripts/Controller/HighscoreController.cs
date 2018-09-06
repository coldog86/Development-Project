using UnityEngine;

namespace _LetsQuiz
{
    public class HighscoreController : Singleton<HighscoreController>
    {
        #region properties

        public string HighScoreData { get; private set; }

        #endregion properties

        #region methods

        public void Load()
        {
            DontDestroyOnLoad(gameObject);

            Debug.Log("[HighscoreController] Load()");

            if (PlayerController.Initialised)
                HighScoreData = PlayerController.Instance.HighScoreJSON;
				Debug.Log ("AllHighScore Data" + HighScoreData);
        }

        public HighScoresContainer ExtractHighScores()
        {
            HighScoresContainer AllHighScorers = JsonUtility.FromJson<HighScoresContainer>(HighScoreData);

            return AllHighScorers;
        }

        #endregion methods
    }
}