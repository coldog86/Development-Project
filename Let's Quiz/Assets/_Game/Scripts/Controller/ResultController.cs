using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace _LetsQuiz
{
    public class ResultController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        public Image backgroundEffect;
        public Text username;
        public Text score;
        public Text rank;
        public Text heading;
        public Text subHeading;

        private FeedbackClick _click;
        private FeedbackMusic _music;
        private PlayerController _playerController;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            _click = FindObjectOfType<FeedbackClick>();
            _music = FindObjectOfType<FeedbackMusic>();
            _playerController = FindObjectOfType<PlayerController>();

            Display();
            ChangeText();
        }

        private void Update()
        {
            // spins background image
            backgroundEffect.transform.Rotate(Vector3.forward * Time.deltaTime * 7.0f);
        }

        #endregion

        #region Display

        private void Display()
        {
            username.text = _playerController.GetUsername();
            score.text = _playerController.userScore.ToString();
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
                Debug.Log("Result Controller: FindRanking() : " + download.text);

                calculateRanking(download.text);
                yield return download;
            } 
        }


        private void calculateRanking(string s)
        {
        	List<string> lines = new List<string>(s.Split(new string[] {"<br>"}, System.StringSplitOptions.RemoveEmptyEntries));
			List<int> list = new List<int>();
        	for(int i = 0; i<lines.Count; i++)
        		list.Add(int.Parse(lines[i]));

        	list.Sort();
        	int ranking = 0;
			for(int i = 0; i<list.Count; i++)
			{
				if(list[i] < _PlayerController.userScore)
					ranking = i + 1;
			}

			rank.text = ranking + " out of " + list.Count;
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

        #region user feedback

        private void ChangeText()
        {
            if (_playerController.scoreStatus.Contains("new high score"))
            {
                heading.text = "Congrats!";
                subHeading.text = "You just got a new a high score!";
            }
            else
            {
                heading.text = "Uh-Oh!";
                subHeading.text = "Someone needs to practise more...";
            }
        }

        #endregion

        #endregion
    }
}
