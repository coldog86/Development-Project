using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace _LetsQuiz
{
    public class AccountController : MonoBehaviour
    {
        private FeedbackClick _click;

        private void Start()
        {
            _click = FindObjectOfType<FeedbackClick>();
        }

        public void BackToMenu()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndexHelper.Menu, LoadSceneMode.Single);
        }
    }
}