using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

namespace _LetsQuiz
{
    public class AnswerButton : MonoBehaviour
    {
        [Header("Component")]
        public Text answerText;
        public Button answerButton;
        public Image buttonImage;

        [Header("Colour")]
        public Color original;
        public Color red;
        public Color green;

        private AnswerData answerData;
        private GameController _gameController;

        void Start()
        {
            _gameController = FindObjectOfType<GameController>();
            buttonImage = GetComponent<Image>();
        }

        public void SetUp(AnswerData data)
        {
            answerData = data;
            answerText.text = answerData.answerText;
        }

        public bool isCorrect(AnswerData data)
        {
            bool isCorrect = data.isCorrect;
            return isCorrect;
        }

        public void HandleAnswerClick()
        {
            if (!_gameController.clicked) //prevents players from selecting multiple answers
            {
                _gameController.clicked = true;

                if (!answerData.isCorrect)
                    changeToRed();
                else
                    changeToGreen();
            }		
        }


        public void changeToGreen()
        {
            StartCoroutine("Green");
        }

        IEnumerator Green()
        {
            buttonImage.color = green; //set wrong push to red
            yield return new WaitForSeconds(1); //give people a chance to see 
            buttonImage.color = original; //change color back
            _gameController.Score(answerData.isCorrect);
        }

        public void changeToRed()
        {
            StartCoroutine("Red");
        }

        IEnumerator Red()
        {
            buttonImage.color = red; //set wrong push to red
            AnswerButton rightButton = _gameController.getCorrectAnswerButton();  //get the correct answer
            rightButton.buttonImage.color = green; //change it to green
            yield return new WaitForSeconds(1); //give people a chance to see 
            buttonImage.color = original; //change wrong answer color back 
            rightButton.buttonImage.color = original; //change correct answer color back
            _gameController.Score(answerData.isCorrect);
        }
    }
}