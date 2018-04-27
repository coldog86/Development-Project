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

        [Header("Components")]
        public Slider timerBar;
        public Image timerImage;
        public Text timerText;
        public Color timerColorMax;
        public Color timerColorMid;
        public Color timerColorMin;

        private float _timeRemaining = 20;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
        }

        private void Update()
        {
            _timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();
        }

        #endregion

        // NOTE : PLACEHOLDER
        public void ReportQuestion()
        {
            FeedbackAlert.Show("Report question");
        }

        // NOTE : PLACEHOLDER
        public void LikeQuestion()
        {
            FeedbackAlert.Show("Like question");
        }

        #region timer specific

        private void UpdateTimeRemainingDisplay()
        {
            var timeRemaining = Mathf.Round(_timeRemaining);
            timerBar.value = timeRemaining;
            timerText.text = timeRemaining.ToString();

            if (timerBar.value > 15)
            {
                timerImage.color = timerColorMax;
                timerText.color = timerColorMax;
            }
            else if (timerBar.value < 16 && timerBar.value > 5)
            {
                timerImage.color = timerColorMid;
                timerText.color = timerColorMid;
            }
            else if (timerBar.value <= 5)
            {
                timerImage.color = timerColorMin;
                timerText.color = timerColorMin;
            }
        }

        #endregion

        public void EndGame()
        {
            SceneManager.LoadScene(BuildIndex.Result, LoadSceneMode.Single);
        }

        #endregion
    }
}

