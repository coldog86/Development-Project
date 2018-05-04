using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace _LetsQuiz
{
    public class AccountController : MonoBehaviour
    {
        #region variables

        private FeedbackClick _click;
        private PlayerController _playerController;
        private Text _warningText;

        #endregion

        #region methods

        private void Awake()
        {
            _click = FindObjectOfType<FeedbackClick>();
            _playerController = FindObjectOfType<PlayerController>();
            _playerController.LoadPlayer();
            _warningText = GameObject.FindGameObjectWithTag("Warning_Text").GetComponent<Text>();
            _warningText.enabled = false;
        }

        private void Start()
        {
            if (_playerController.GetPlayerType() == PlayerStatus.Guest)
                _warningText.enabled = true;
            else
                _warningText.enabled = false;
        }

        public void BackToMenu()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion
    }
}