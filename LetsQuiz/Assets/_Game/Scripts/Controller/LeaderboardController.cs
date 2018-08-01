﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _LetsQuiz
{
    public class LeaderboardController : MonoBehaviour
    {
        #region variables

        [Header("Playe High Scorers")]
        private HighScoresContainer allHighScores;
        public SimpleObjectPool highScorerObjectPool;
        public Transform highScorerParent;
        private HighscoreController _highScoreController;
        private string highScoreData;

        private PlayerController _playerController;
        private DataController _dataController;

        [Header("Question High Scorers")]
        private QuestAndSub[] _questandSub;
        public SimpleObjectPool questionHighscoreObjectPool;
        public Transform questionHighscoreParent;

        [Header("Total Correct Answers Scorers")]
        public SimpleObjectPool TotalCorrectObjectPool;
        public Transform TotalCorrectParent;

        //gameobject for overall highscore
        private List<GameObject> highScorerGameObjects = new List<GameObject>();

        //gameobject for submitted questions
        private List<GameObject> questionHighscoreObjects = new List<GameObject>();

        //gameobjecy for total questions correct by players
        private List<GameObject> totalQuestionsCorrectObjects = new List<GameObject>();

        #endregion variables

        #region methods

        #region unity

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _highScoreController = FindObjectOfType<HighscoreController>();

            _highScoreController.Load();
            allHighScores = _highScoreController.extractHighScores();

            ShowHighScorers(allHighScores);


            //get the QuestandSub highscorers
            _questandSub = _playerController.GetQuestandSubData();
            ShowQuestionHighScorers(_questandSub);

            ShowTotalQuestionsCorrect(allHighScores);
        }

        #endregion unity

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

        //removes all Player Highscore LeaderboardEntry Objects from the scene
        private void RemoveHighScorers()
        {
            while (highScorerGameObjects.Count > 0)
            {
                highScorerObjectPool.ReturnObject(highScorerGameObjects[0]);
                highScorerGameObjects.RemoveAt(0);
            }
        }

        //show/sort  Player Questions by rating
        private void ShowQuestionHighScorers(QuestAndSub[] unsortedQuestions)
        {
            QuestAndSub[] sortedQuestionsByRating = unsortedQuestions.OrderBy(c => c.getRating()).ToArray();


            //for some reason the sorted array is in reverse order, so the for loop runs from the last 10 items.
            for (int i = sortedQuestionsByRating.Length - 1; i > sortedQuestionsByRating.Length - 11; i--)
            {
                GameObject questionHighScoreObject = questionHighscoreObjectPool.GetObject(); //create new GameObejct
                QuestAndSub currentQuestionHighscore = sortedQuestionsByRating[i];                      //get current highscorer

                questionHighscoreObjects.Add(questionHighScoreObject);
                questionHighScoreObject.transform.SetParent(questionHighscoreParent);
                LeaderboardEntry leaderBoardEntry = questionHighScoreObject.GetComponent<LeaderboardEntry>();

                leaderBoardEntry.SetUp(currentQuestionHighscore.QuestionText, currentQuestionHighscore.Rating.ToString()); //pass in the data of current HighScorer
            }
        }

        //removes all Question Highscore Obejcts from the scene
        private void RemoveQuestionHighscores()
        {
            while (questionHighscoreObjects.Count > 0)
            {
                questionHighscoreObjectPool.ReturnObject(questionHighscoreObjects[0]);
                questionHighscoreObjects.RemoveAt(0);
            }
        }


        //show most correct answers
        private void ShowTotalQuestionsCorrect(HighScoresContainer allHighScorers)
        {
            RemoveTotalQuestionsCorrect(); //clear leaderboard to start

            //sort scores by totalScore.
            HighScoresObject[] sorted = allHighScorers.allHighScorers.OrderBy(c => c.getTotalCorrect()).ToArray();

            //for some reason the sorted array is in reverse order, so the for loop runs from the last 10 items.
            for (int i = sorted.Length - 1; i > sorted.Length - 11; i--)
            {
                GameObject totalCorrectgameObject = TotalCorrectObjectPool.GetObject(); //create new GameObejct
                HighScoresObject currentHighScore = sorted[i]; 						//get current highscorer

                highScorerGameObjects.Add(totalCorrectgameObject);
                totalCorrectgameObject.transform.SetParent(TotalCorrectParent);
                LeaderboardEntry leaderBoardEntry = totalCorrectgameObject.GetComponent<LeaderboardEntry>();

                leaderBoardEntry.SetUp(currentHighScore.userName, currentHighScore.questionsRight); //pass in the data of current HighScorer
            }
        }

        //remove most correct answers objects
        private void RemoveTotalQuestionsCorrect()
        {
            while (totalQuestionsCorrectObjects.Count > 0)
            {
                TotalCorrectObjectPool.ReturnObject(totalQuestionsCorrectObjects[0]);
                totalQuestionsCorrectObjects.RemoveAt(0);
            }
        }


        #endregion high score specific

        #region navigation specific

        public void BackToMenu()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion navigation specific

        #endregion methods
    }
}