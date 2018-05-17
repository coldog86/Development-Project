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
        public Text userName;
        public Text score;
        public Text rank;

        private FeedbackClick _click;
        private FeedbackMusic _music;
        private PlayerController _PlayerController;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            _click = FindObjectOfType<FeedbackClick>();
            _music = FindObjectOfType<FeedbackMusic>();
			_PlayerController = FindObjectOfType<PlayerController>();

            display();
        }

        private void Update()
        {
            backgroundEffect.transform.Rotate(Vector3.forward * Time.deltaTime * 7.0f);
        }

        #endregion

        #region Display

        private void display()
        {
			userName.text = _PlayerController.GetUsername();
			score.text = _PlayerController.userScore.ToString();
        }


		public IEnumerator FindRanking()
        {
			float _downloadTimer = 5.0f;
            WWW download = new WWW(ServerHelper.Host + ServerHelper.Ranking);
            while (!download.isDone)
            {
                if (_downloadTimer < 0)
                {
                    Debug.LogError("Server time out.");
                    break;
                }
                _downloadTimer -= Time.deltaTime;

                yield return null;
            }

            if (!download.isDone || download.error != null)
            {
                /* if we cannot connect to the server or there is some error in the data, 
                 * check the prefs for previously saved questions */
                Debug.LogError(download.error);
                Debug.Log("Failed to hit the server.");
            }
            else
            { 
                // we got the string from the server, it is every question in JSON format
                Debug.Log(download.text);

                string rankingsAsJSON = download.text;

                yield return download;


            } 
        }





        #endregion



        #region user interaction

        public void ShareResults()
        {
            _click.Play();
            FeedbackAlert.Show("Share results");
        }

        public void BackToMenu()
        {
            _click.Play();
            _music.PlayBackgroundMusic();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion

        #endregion
    }
}
