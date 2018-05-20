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

