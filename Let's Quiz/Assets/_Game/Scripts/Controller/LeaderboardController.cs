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

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            _click = FindObjectOfType<FeedbackClick>();

            _playerController = FindObjectOfType<PlayerController>();

            _playerController.Load();

            _warningPanel = GameObject.FindGameObjectWithTag("Panel_Warning");
            _warningPanel.SetActive(false);

            _userPanel = GameObject.FindGameObjectWithTag("Panel_User");
            _userPanel.SetActive(false);
        }

        private void Start()
        {

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

