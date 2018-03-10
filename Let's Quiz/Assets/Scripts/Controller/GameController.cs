using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Data;
using Helper;
using Object;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        [Header("Component")]
        public Text QuestionDisplayText;

        public Text ScoreDisplayText;
        public Text TimeRemainingDisplayText;
        public SimpleObjectPool AnswerButtonObjectPool;
        public Transform AnswerButtonParent;
        public GameObject QuestionDisplay;
        public GameObject RoundEndDisplay;

        [Header("Setting")]
        public int MenuSceneIndex = 1;

        private DataController _dataController;
        private RoundData _currentRoundData;
        private QuestionData[] _questionPool;

        private bool _isRoundActive;
        private float _timeRemaining;
        private int _questionIndex;
        private int _playerScore;
        private readonly List<GameObject> _answerButtonGameObjects = new List<GameObject>();

        private void Start()
        {
            if (!_dataController)
            {
                Debug.LogError("Data Controller Instance cannot be null. Attempting to find data controller object.");
                _dataController = FindObjectOfType<DataController>();
            }

            _currentRoundData = _dataController.GetCurrentRoundData();
            _questionPool = _currentRoundData.Questions;
            _timeRemaining = _currentRoundData.TimeLimitSeconds;

            UpdateTimeRemainingDisplay();

            _playerScore = 0;
            _questionIndex = 0;

            ShowQuestion();
            _isRoundActive = true;
        }

        private void ShowQuestion()
        {
            RemoveAnswerButton();

            var questionData = _questionPool[_questionIndex];
            QuestionDisplayText.text = questionData.QuestionText;

            foreach (var answer in questionData.Answers)
            {
                var answerButtonGameObject = AnswerButtonObjectPool.GetObject();
                _answerButtonGameObjects.Add(answerButtonGameObject);
                answerButtonGameObject.transform.SetParent(AnswerButtonParent);

                var answerButton = answerButtonGameObject.GetComponent<AnswerButtonHelper>();
                answerButton.Setup(answer);
            }
        }

        private void RemoveAnswerButton()
        {
            while (_answerButtonGameObjects.Count > 0)
            {
                AnswerButtonObjectPool.ReturnObject(_answerButtonGameObjects[0]);
                _answerButtonGameObjects.RemoveAt(0);
            }
        }

        public void AnswerButtonClicked(bool isCorrect)
        {
            if (isCorrect)
            {
                _playerScore += _currentRoundData.CorrectAnswerPoints;
                ScoreDisplayText.text = "Score: " + _playerScore.ToString();
            }

            if (_questionPool.Length > _questionIndex + 1)
            {
                _questionIndex++;
                ShowQuestion();
            }
            else
            {
                EndRound();
            }
        }

        public void EndRound()
        {
            _isRoundActive = false;

            QuestionDisplay.SetActive(false);
            RoundEndDisplay.SetActive(true);
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene(MenuSceneIndex);
        }

        private void UpdateTimeRemainingDisplay()
        {
            TimeRemainingDisplayText.text = "Time: " + Mathf.Round(_timeRemaining).ToString();
        }

        private void Update()
        {
            if (!_isRoundActive) return;

            _timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (_timeRemaining <= 0f)
            {
                EndRound();
            }
        }
    }
}