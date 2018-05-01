using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class ResultController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        public Image backgroundEffect;

        #endregion

        #region methods

        #region unity

        private void Update()
        {
            backgroundEffect.transform.Rotate(Vector3.forward * Time.deltaTime * 7.0f);
        }

        #endregion

        #region user interaction

        public void ShareResults()
        {
            FeedbackAlert.Show("Share Results...");
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion

        #endregion
    }
}
