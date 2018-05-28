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
        public Text score;
        public Text username;
        public Text rankText;
        public Text rank;
        public Text worldText;

        private FeedbackClick _click;
        private FeedbackMusic _music;
        private PlayerController _playerController;

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            _click = FindObjectOfType<FeedbackClick>();
            _music = FindObjectOfType<FeedbackMusic>();
            _playerController = FindObjectOfType<PlayerController>();                
        }

        private void Start()
        { 
            if (_playerController.GetPlayerType() == PlayerStatus.LoggedIn)
            {
                score.enabled = true;
                username.enabled = true;
                rank.enabled = true;
                rankText.enabled = true;
                worldText.enabled = true;
            }
            else
            {
                score.enabled = false;
                username.enabled = true;
                rank.enabled = true;
                rankText.enabled = true;
                worldText.enabled = false;
            }

            StartCoroutine(FindRanking());
            Display();
        }

        private void Update()
        {
            // spins background image
            backgroundEffect.transform.Rotate(Vector3.forward * Time.deltaTime * 7.0f);
        }

        #endregion

        #region user feedback

        private void Display()
        {
            if (_playerController.GetPlayerType() == PlayerStatus.LoggedIn)
            {
                score.text = _playerController.userScore.ToString();
                username.text = _playerController.GetUsername();
            }
            else
            {
                username.text = _playerController.GetUsername();
                rankText.text = "Your final score was";
                rank.text = _playerController.userScore.ToString();
            }
        }

        #endregion

        #region rank specific

        public IEnumerator FindRanking()
        {
            float _downloadTimer = 5.0f;

            WWW download = new WWW(ServerHelper.Host + ServerHelper.Ranking);

            while (!download.isDone)
            {
                if (_downloadTimer < 0)
                {
                    Debug.LogError("ResultController : FindRanking(): " + download.error);
                    break;
                }
                _downloadTimer -= Time.deltaTime;

                yield return null;
            }

            if (!download.isDone || download.error != null)
            {
                /* if we cannot connect to the server or there is some error in the data, 
                 * check the prefs for previously saved questions */
                Debug.LogError("ResultController : FindRanking(): " + download.error);
                Debug.Log("Failed to hit the server.");
                yield return null;
            }
            else
            { 
                // we got the string from the server, it is every question in JSON format
                Debug.Log("ResultController: FindRanking() : " + download.text);
                yield return download;
                calculateRanking(download.text);
            } 
        }

        private void calculateRanking(string s)
        {
            var lines = new List<string>(s.Split(new string[] { "<br>" }, System.StringSplitOptions.RemoveEmptyEntries));
            var list = new List<int>();

            for (int i = 0; i < lines.Count; i++)
                list.Add(int.Parse(lines[i]));

            list.Sort();
            int ranking = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] < _playerController.userScore)
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

        #endregion
    }
}
