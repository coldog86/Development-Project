using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//TODO: FIND WAY TO GIVE REAL TIME FEEDBACK ON ANSWER CORRECTNESS

public class GameController : MonoBehaviour
{
    [Header("Component")]
    public Text questionDisplayText;
    public Text scoreDisplayText;
    public Text highestScoreDisplayText;
    public Slider timerBar;
    public Image timerImage;
    public Color timerColorMax;
    public Color timerColorMid;
    public Color timerColorMin;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;
    public GameObject questionDisplay;
    public GameObject roundEndDisplay;

    [Header("Setting")]
    public const int MenuSceneIndex = 1;

    private DataController _dataController;
    private RoundData _currentRoundData;
    private QuestionData[] _questionPool;

    private bool _isRoundActive;
    private float _timeRemaining;
    private int _questionIndex;
    private int _playerScore;
    private List<GameObject> _answerButtonGameObjects = new List<GameObject>();

    private void Start()
    {
        _dataController = FindObjectOfType<DataController>();

        _currentRoundData = _dataController.GetCurrentRoundData();
        _questionPool = _currentRoundData.questions;
        _timeRemaining = _currentRoundData.timeLimitSeconds;

        timerBar.maxValue = _currentRoundData.timeLimitSeconds;
        timerBar.value = _timeRemaining;
        timerBar.wholeNumbers = true;
        timerImage.color = timerColorMax;

        UpdateTimeRemainingDisplay();

        _playerScore = 0;
        _questionIndex = 0;

        ShowQuestion();
        _isRoundActive = true;
    }

    private void Update()
    {
        if (!_isRoundActive)
            return;

        _timeRemaining -= Time.deltaTime;

        UpdateTimeRemainingDisplay();

        if (_timeRemaining <= 0.0f)
            EndRound();
    }

    private void ShowQuestion()
    {
        RemoveAnswerButton();

        var questionData = _questionPool[_questionIndex];
        questionDisplayText.text = questionData.questionText;

        foreach (var answer in questionData.answers)
        {
            var answerButtonGameObject = answerButtonObjectPool.GetObject();
            _answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);

            var answerButton = answerButtonGameObject.GetComponent<AnswerButtonHelper>();
            answerButton.SetUp(answer);
        }
    }

    private void RemoveAnswerButton()
    {
        while (_answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(_answerButtonGameObjects[0]);
            _answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)
        {
            _playerScore += _currentRoundData.correctAnswerPoints;
            scoreDisplayText.text = _playerScore.ToString();
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

    private void UpdateTimeRemainingDisplay()
    {
        timerBar.value = Mathf.Round(_timeRemaining);

        if (timerBar.value > 20)
            timerImage.color = timerColorMax;
        else if (timerBar.value < 21 && timerBar.value > 9)
            timerImage.color = timerColorMid;
        else if (timerBar.value <= 9)
            timerImage.color = timerColorMin;
    }

    public void EndRound()
    {
        _isRoundActive = false;

        _dataController.SetPlayerHighestScore(_playerScore);
        highestScoreDisplayText.text = _dataController.GetPlayerHighestScore().ToString();

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(MenuSceneIndex, LoadSceneMode.Single);
    }
}