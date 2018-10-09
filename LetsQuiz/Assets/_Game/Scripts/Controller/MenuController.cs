using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class MenuController : Singleton<MenuController>
    {
        #region variables

        [Header("Component")]
        public Button accountButton;

        public Button leaderboardButton;
        public Button submitQuestionButton;

        private Text _usernameText;
        private FeedbackMusic _music;
        private GetAllQuestions _questionDownload;

        #endregion variables

        #region methods

        #region unity

        protected override void Awake()
        {
            if (Initialised)
                return;

            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            _usernameText = GameObject.FindGameObjectWithTag("Username_Text").GetComponent<Text>();

            var playerType = 0;

            if (PlayerController.Initialised)
                playerType = PlayerController.Instance.GetPlayerType();

            if (PlayerPrefs.HasKey(DataHelper.PlayerDataKey.USERNAME) && (playerType == PlayerStatus.LoggedIn || playerType == PlayerStatus.Guest))
                _usernameText.text = PlayerController.Instance.GetUsername();

            if (playerType == PlayerStatus.Guest)
            {
                accountButton.gameObject.SetActive(false);
                leaderboardButton.gameObject.SetActive(false);
                submitQuestionButton.gameObject.SetActive(false);
            }
        }

        // TODO what is this for, I have seen a few private start() methods, when are they called, do they behave like a public Start()?
        // NOTE : does the same as public start() - just better programming pratise
        private void Start()
        {
            _music = FindObjectOfType<FeedbackMusic>();
            _music.PlayBackgroundMusic();
            _questionDownload = FindObjectOfType<GetAllQuestions>();
            Destroy(_questionDownload);
        }

        #endregion unity

        #region game specific

        public void StartGame()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Game, LoadSceneMode.Single);
        }

        public void GoToGameLobby()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.GameLobby, LoadSceneMode.Single);
        }

        #endregion game specific

        #region navigation specific

        public void OpenAccount()
        {
            FeedbackClick.Play();
            Destroy(gameObject);
            SceneManager.LoadScene(BuildIndex.Account, LoadSceneMode.Single);
        }

        public void OpenLeaderboard()
        {
            FeedbackClick.Play();
            Destroy(gameObject);
            SceneManager.LoadScene(BuildIndex.Leaderboard, LoadSceneMode.Single);
        }

        public void OpenSubmitQuestion()
        {
            FeedbackClick.Play();
            Destroy(gameObject);
            SceneManager.LoadScene(BuildIndex.SubmitQuestion, LoadSceneMode.Single);
        }

        public void OpenSetting()
        {
            FeedbackClick.Play();
            Destroy(gameObject);
            SceneManager.LoadScene(BuildIndex.Settings, LoadSceneMode.Single);
        }

        public void OpenNotification()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.NotificationHelper, LoadSceneMode.Single);
        }

        public void Logout()
        {
            FeedbackClick.Play();
            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to log out?", "Log out", "Cancel", OpenLogin, FeedbackTwoButtonModal.Hide);
        }

        private void OpenLogin()
        {
            if (FB.IsLoggedIn)
                FB.LogOut();

            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(BuildIndex.Login, LoadSceneMode.Single);
        }

        public void Quit()
        {
            FeedbackClick.Play();

            if (FirebaseController.Initialised)
            {
                if (DataController.Instance.NewPlayer)
                    FirebaseController.Instance.CreateNotificationDelay(PlayerController.Instance.GetToken(), "Let's Quiz", "Thanks for playing!");

                if (DataController.Instance.SubmittedQuestion)
                    FirebaseController.Instance.CreateNotificationDelay(PlayerController.Instance.GetToken(), "Let's Quiz", "Thanks for the question!");
            }

            DataController.Instance.NewPlayer = false;
            DataController.Instance.SubmittedQuestion = false;
            FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", Application.Quit, FeedbackTwoButtonModal.Hide);
        }

        #endregion navigation specific

        #endregion methods
    }
}