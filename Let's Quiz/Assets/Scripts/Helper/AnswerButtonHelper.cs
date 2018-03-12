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

        private GameController _gameController;
        private AnswerData _answerData;
        

        private void Start()
        {
           _gameController = FindObjectOfType<GameController>();
        }

        public void Setup(AnswerData data)
        {
            _answerData = data;
            AnswerText.text = _answerData.answerText;
        }

        public void HandleClick()
        {
            _gameController.AnswerButtonClicked(_answerData.isCorrect);
        }
    }
}