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

        [Header("Connection")]
        public float connectionTimer = 0;
        public const float connectionTimeLimit = 10000.0f;

        private FeedbackClick _click;
        private PlayerController _playerController;

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            _click = FindObjectOfType<FeedbackClick>();

            _playerController = FindObjectOfType<PlayerController>();

            _playerController.Load();
        }

        #endregion

        #region submit specific

        public void Submit()
        {
            _click.Play();

            var question = questionInput.text;
            var correctAnswer = correctInput.text;
            var wrong1Answer = wrong1Input.text;
            var wrong2Answer = wrong2Input.text;
            var wrong3Answer = wrong3Input.text;

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

            if (!string.IsNullOrEmpty(question) && !string.IsNullOrEmpty(correctAnswer) && !string.IsNullOrEmpty(wrong1Answer) && !string.IsNullOrEmpty(wrong2Answer) && !string.IsNullOrEmpty(wrong3Answer))
            {
                if (ValidSubmission(question, correctAnswer, wrong1Answer, wrong2Answer, wrong3Answer))
                {
                    FeedbackAlert.Show("Question submitted sucessfully.");
                    questionInput.text = "";
                    correctInput.text = "";
                    wrong1Input.text = "";
                    wrong2Input.text = "";
                    wrong3Input.text = "";
                }
                else
                    FeedbackAlert.Show("Question submitted unucessfully.");
            }
        }

        private bool ValidSubmission(string question, string correctAnswer, string wrong1Answer, string wrong2Answer, string wrong3Answer, string category = "User Submitted Question")
        {
            Debug.Log("SubmitController: ValidSubmisstion() : Attempting to Submit");

            WWWForm form = new WWWForm();

            //form.AddField("usernamePost", _playerController.GetUsername());
            //form.AddField("idPost", _playerController.GetId());
            form.AddField("questionText", question);
            form.AddField("correctAnswer", correctAnswer);
            form.AddField("wrong1", wrong1Answer);
            form.AddField("wrong2", wrong2Answer);
            form.AddField("wrong3", wrong3Answer);
            form.AddField("catgory", category);

            WWW submitQuestion = new WWW(ServerHelper.Host + ServerHelper.SubmitUserQuestion, form);

            while (!submitQuestion.isDone)
            { 
                connectionTimer += Time.deltaTime;
                FeedbackAlert.Show("Attempting to submit question.");

                if (connectionTimer > connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("SubmitController : ValidSubmission() : " + submitQuestion.error);
                    Debug.Log(submitQuestion.text);
                    return false;
                }

                // extra check just to ensure a stream error doesn't come up
                if (connectionTimer > connectionTimeLimit || submitQuestion.error != null)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("SubmitController : ValidSubmission() : " + submitQuestion.error);
                    Debug.Log(submitQuestion.text);
                    return false;
                }
            }

            if (submitQuestion.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.Log("SubmitController : ValidSubmission() : " + submitQuestion.error);
                return false;
            }

            if (submitQuestion.isDone)
            {
                if (!string.IsNullOrEmpty(submitQuestion.text))
                {
                    Debug.Log(submitQuestion.text);
                    return true;
                }
                else
                    return false;
            }
            return false;
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
