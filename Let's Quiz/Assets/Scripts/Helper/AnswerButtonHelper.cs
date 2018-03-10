using UnityEngine;
using UnityEngine.UI;
using Controller;
using Data;

namespace Helper
{
    public class AnswerButtonHelper : MonoBehaviour
    {
        [Header("Component")]
        public Text AnswerText;

        private AnswerData _answerData;
        private GameController _gameController = null;

        private void Start()
        {
            if (_gameController) return;

            Debug.LogError("Game Controller Instance cannot be null. Attempting to find game controller object.");
            _gameController = FindObjectOfType<GameController>();
        }

        public void Setup(AnswerData data)
        {
            _answerData = data;
            AnswerText.text = _answerData.AnswerText;
        }

        public void HandleClick()
        {
            _gameController.AnswerButtonClicked(_answerData.IsCorrect);
        }
    }
}