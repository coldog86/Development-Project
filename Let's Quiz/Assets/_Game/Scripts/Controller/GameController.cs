using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

namespace _LetsQuiz
{
    public class GameController : MonoBehaviour
    {
        #region variables

        [Header("Timer")]
        public Slider timerBar;
        public Image timerImage;
        public Color timerColorMax;
        public Color timerColorMid;
        public Color timerColorMin;

        [Header("Question")]
        public Text questionText;
        public Text scoreText;

        private FeedbackClick _click;
        private FeedbackMusic _music;
        private float _timeRemaining = 20;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            _click = FindObjectOfType<FeedbackClick>();
            _music = FindObjectOfType<FeedbackMusic>();
        }

        private void Update()
        {
            _timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (_timeRemaining <= 0)
                EndGame();
        }

        #endregion

        // NOTE : placeholder
        public void ReportQuestion()
        {
            _click.Play();
            FeedbackAlert.Show("Report question");
        }

        // NOTE : placeholder
        public void LikeQuestion()
        {
            _click.Play();
            FeedbackAlert.Show("Like question");
        }

        #region timer specific

        private void UpdateTimeRemainingDisplay()
        {
            var timeRemaining = Mathf.Round(_timeRemaining);
            timerBar.value = timeRemaining;

            if (timerBar.value > 15)
                timerImage.color = timerColorMax;
            else if (timerBar.value < 16 && timerBar.value > 6)
                timerImage.color = timerColorMid;
            else if (timerBar.value <= 5)
                timerImage.color = timerColorMin;
        }

        #endregion

        public void EndGame()
        {
            _music.Stop();
            SceneManager.LoadScene(BuildIndex.Result, LoadSceneMode.Single);
        }

        #endregion
    }
}

