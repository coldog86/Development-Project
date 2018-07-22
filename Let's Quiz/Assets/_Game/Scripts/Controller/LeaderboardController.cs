using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _LetsQuiz
{
    public class LeaderboardController : MonoBehaviour
    {
        #region variables

        [Header("High Scorers")]
        private HighScoresContainer allHighScores;
        public SimpleObjectPool highScorerObjectPool;
        public Transform highScorerParent;
        private HighscoreController _highScoreController;
        private string highScoreData;
		private QuestAndSub[] _questandSub;

        private PlayerController _playerController;
        private DataController _dataController;
        private FeedbackClick _click;

        private List<GameObject> highScorerGameObjects = new List<GameObject>();

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            _click = FindObjectOfType<FeedbackClick>();
        }

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _highScoreController = FindObjectOfType<HighscoreController>();

            _highScoreController.Load();
            allHighScores = _highScoreController.extractHighScores();

            ShowHighScorers(allHighScores);

			//get the QuestandSub highscorers
			_questandSub = _playerController.GetQuestandSubData();
			ShowQuestionHighScorers (_questandSub);

        }

        #endregion

        #region high score specific

        //sorts highScorers and displays top 10 in highScorerParent using LeaderboardEntry prefabs.
        private void ShowHighScorers(HighScoresContainer allHighScorers)
        {
            RemoveHighScorers(); //clear leaderboard to start

            //sort scores by totalScore. 
            HighScoresObject[] sorted = allHighScorers.allHighScorers.OrderBy(c => c.getTotalScoreInt()).ToArray();

            //for some reason the sorted array is in reverse order, so the for loop runs from the last 10 items. 
            for (int i = sorted.Length - 1; i > sorted.Length - 11; i--)
            {
                GameObject highScorerGameObject = highScorerObjectPool.GetObject(); //create new GameObejct
                HighScoresObject currentHighScore = sorted[i]; 						//get current highscorer

                highScorerGameObjects.Add(highScorerGameObject);  
                highScorerGameObject.transform.SetParent(highScorerParent);
                LeaderboardEntry leaderBoardEntry = highScorerGameObject.GetComponent<LeaderboardEntry>();

                leaderBoardEntry.SetUp(currentHighScore.userName, currentHighScore.totalScore); //pass in the data of current HighScorer
            }
        }

        //removes all LeaderboardEntry Objects from the scene
        void RemoveHighScorers()
        {
            while (highScorerGameObjects.Count > 0)
            {
                highScorerObjectPool.ReturnObject(highScorerGameObjects[0]);
                highScorerGameObjects.RemoveAt(0);
            }
        }


		//show/sort  Player Questions by rating
		private void ShowQuestionHighScorers(QuestAndSub[] unsortedQuestions) {

			QuestAndSub[] sortedQuestionsByRating = unsortedQuestions.OrderBy(c => c.getRating()).ToArray();

			Debug.Log("Rating " + sortedQuestionsByRating[20].Rating);

		}



        #endregion





        #region navigation specific

        public void BackToMenu()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion

        #endregion
    }
}

