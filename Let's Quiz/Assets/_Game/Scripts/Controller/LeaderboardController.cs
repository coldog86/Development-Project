using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class LeaderboardController : MonoBehaviour
    {
        #region variables

        private FeedbackClick _click;
        private PlayerController _playerController;
        private GameObject _warningPanel;
        private GameObject _userPanel;
		private HighScoresContainer allHighScores;
		private DataController _dataController;
		private string highScoreData;
		private HighscoreController _highScoreController;

		[Header("HighScorers")]
		public SimpleObjectPool highScorerObjectPool;
		public Transform highScorerParent;

		private List<GameObject> highScorerGameObjects = new List<GameObject>();

        #endregion

        #region methods

        #region unity

       

        private void Start()
        {

			_playerController = FindObjectOfType<PlayerController>();
			_highScoreController = FindObjectOfType<HighscoreController>();

			_highScoreController.Load ();
			allHighScores = _highScoreController.extractHighScores();
			Debug.Log (allHighScores.allHighScorers.Length);

			showHighScorers(allHighScores);

            if (_playerController.GetPlayerType() != PlayerStatus.LoggedIn)
            {
                _warningPanel.SetActive(true);
                _userPanel.SetActive(false);
            }
            else
            {
                _warningPanel.SetActive(false);
                _userPanel.SetActive(true);
            }
				
        }

		private void Awake()
		{
			_click = FindObjectOfType<FeedbackClick>();



			_warningPanel = GameObject.FindGameObjectWithTag("Panel_Warning");
			_warningPanel.SetActive(false);

			_userPanel = GameObject.FindGameObjectWithTag("Panel_User");
			_userPanel.SetActive(false);
		}


		private void showHighScorers(HighScoresContainer allHighScorers) {


			//need to sort highScorers into order

			removeHighScorers (); //clear leaderboard to start

			GameObject highScorerGameObject = highScorerObjectPool.GetObject();

			for (int i = 0; i < allHighScorers.allHighScorers.Length; i++) {

				HighScoresObject currentHighScore = allHighScorers.allHighScorers [i]; //get current highscorer

				highScorerGameObjects.Add (highScorerGameObject);  //this will need to repeat for however many items in list
				highScorerGameObject.transform.SetParent(highScorerParent);
				LeaderboardEntry leaderBoardEntry = highScorerGameObject.GetComponent<LeaderboardEntry> ();

				leaderBoardEntry.SetUp(currentHighScore.userName, currentHighScore.totalScore); //pass in the data of current HighScorer

			}

		}

		void removeHighScorers() {

			while (highScorerGameObjects.Count > 0) {

				highScorerObjectPool.ReturnObject (highScorerGameObjects [0]);
				highScorerGameObjects.RemoveAt (0);
			}
		}

        #endregion

        #region navigatin specific

        public void BackToMenu()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }


        #endregion

        #endregion
    }
}

