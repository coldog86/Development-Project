using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class SubmitController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        public InputField questionInput;
        public InputField correctInput;
        public InputField wrong1Input;
        public InputField wrong2Input;
        public InputField wrong3Input;
        public Dropdown categorySelection;

        [SerializeField] private Text _questionText;
        [SerializeField] private Text _correctAnswerText;
        [SerializeField] private Text _wrongAnswer1Text;
        [SerializeField] private Text _wrongAnswer2Text;
        [SerializeField] private Text _wrongAnswer3Text;

        [Header("Connection")]
        private float _connectionTimer = 0;
        private const float _connectionTimeLimit = 10000.0f;

        #endregion variables

        #region methods

        #region unity

        private void Awake()
        {
            if (PlayerController.Initialised)
                PlayerController.Instance.Load();

            List<string> categories = new List<string>();

            if (QuestionController.Initialised)
                categories = QuestionController.Instance.GetAllCategories();

            categories.Insert(0, "Select a category");
            categorySelection.AddOptions(categories);
        }

        #endregion unity

        #region submit specific

        public void Submit()
        {
            FeedbackClick.Play();

            var question = questionInput.text;
            var correctAnswer = correctInput.text;
            var wrong1Answer = wrong1Input.text;
            var wrong2Answer = wrong2Input.text;
            var wrong3Answer = wrong3Input.text;
            var category = categorySelection.options[categorySelection.value].text;

            if (string.IsNullOrEmpty(question))
                FeedbackAlert.Show("Question cannot be empty.");

            if (string.IsNullOrEmpty(correctAnswer))
                FeedbackAlert.Show("Correct Answer cannot be empty.");

            if (string.IsNullOrEmpty(wrong1Answer))
                FeedbackAlert.Show("Wrong Answer 1 cannot be empty.");

            if (string.IsNullOrEmpty(wrong2Answer))
                FeedbackAlert.Show("Wrong Answer 2 cannot be empty.");

            if (string.IsNullOrEmpty(wrong3Answer))
                FeedbackAlert.Show("Wrong Answer 3 cannot be empty.");

            if (category == "Select a category")
                FeedbackAlert.Show("A category must be selected.");

            if (!string.IsNullOrEmpty(question) && !string.IsNullOrEmpty(correctAnswer) && !string.IsNullOrEmpty(wrong1Answer) && !string.IsNullOrEmpty(wrong2Answer) && !string.IsNullOrEmpty(wrong3Answer) && !string.IsNullOrEmpty(category))
            {
                if (ValidSubmission(question, correctAnswer, wrong1Answer, wrong2Answer, wrong3Answer, category))
                {
                    SceneManager.LoadScene(BuildIndex.SubmitQuestion);
                    FeedbackAlert.Show("Submission Succesful.");
#if !UNITY_EDITOR
                    SendNotification();
#endif
                }
            }
        }

        private bool ValidSubmission(string question, string correctAnswer, string wrong1Answer, string wrong2Answer, string wrong3Answer, string category)
        {
            FeedbackAlert.Show("Submitting question...");

            WWWForm form = new WWWForm();

            form.AddField("questionText", question);
            form.AddField("correctAnswer", correctAnswer);
            form.AddField("wrong1", wrong1Answer);
            form.AddField("wrong2", wrong2Answer);
            form.AddField("wrong3", wrong3Answer);
            form.AddField("catagory", category);

            bool complete = false;

            WWW submitQuestion = new WWW(ServerHelper.Host + ServerHelper.SubmitUserQuestion, form);

            _connectionTimer += Time.deltaTime;

            while (!submitQuestion.isDone)
            {
                if (_connectionTimer > _connectionTimeLimit)
                {
                    Debug.LogErrorFormat("[{0}] ValidSubmission() : {1}", GetType().Name, submitQuestion.error);
                    Debug.Log(submitQuestion.text);
                    complete = false;
                    FeedbackAlert.Show("Server time out.");
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || submitQuestion.error != null)
                {
                    Debug.LogErrorFormat("[{0}] ValidSubmission() : {1}", GetType().Name, submitQuestion.error);
                    Debug.Log(submitQuestion.text);
                    complete = false;
                    FeedbackAlert.Show("Server time out.");
                }
            }

            if (submitQuestion.error != null)
            {
                Debug.LogErrorFormat("[{0}] ValidSubmission() : {1}", GetType().Name, submitQuestion.error);
                complete = false;
                FeedbackAlert.Show("Connection error. Please try again.");
            }

            if (submitQuestion.isDone)
            {
                if (!string.IsNullOrEmpty(submitQuestion.text))
                {
                    Debug.LogFormat("[{0}] ValidSubmission() : Submission {1}", GetType().Name, submitQuestion.text);
                    complete = true;
                }
                else
                    complete = false;
            }

            FeedbackAlert.Hide();

            return complete;
        }

        public void CategorySelected()
        {
            string category = categorySelection.options[categorySelection.value].text;
        }

        private void SendNotification()
        {
            StartCoroutine(Send());
        }

        private IEnumerator Send()
        {
            _connectionTimer = 0.0f;

            var form = new WWWForm();
            form.AddField("token", PlayerController.Instance.GetToken());
            form.AddField("title", "Let's Quiz");
            form.AddField("body", "Thanks for the question!");

            var request = new WWW(ServerHelper.Host + ServerHelper.SendNotificationDelay, form);

            _connectionTimer += Time.deltaTime;

            while (!request.isDone)
            {
                if (_connectionTimer > _connectionTimeLimit)
                    yield return null;

                if (_connectionTimer > _connectionTimeLimit || request.error != null)
                    yield return null;

                if (request.error != null)
                    yield return null;
            }

            if (request.isDone && request.error != null)
                yield return null;

            if (request.isDone)
                yield return request.text;
        }

        #endregion submit specific

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