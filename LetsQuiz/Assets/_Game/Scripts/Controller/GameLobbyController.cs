using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class GameLobbyController : Singleton<GameLobbyController>
    {
        #region variables

        [Header("Components")]
        public GameObject CatagoryPopUp;
        public Dropdown CatagoryDropDown;
        public GameObject CatAckPanel;
        public GameObject CatSelectPanel;
        public GameObject BackgroundStuff;
        public Text CatagoryText;

        private CheckForOpenGames _checkForOpenGames;
        private MenuController _menuController;
        private CheckForPlayerExistingGames _checkForPlayerExistingGames;

        private int[] _gameNumbers;
        private List<string> _catagoryList;

        #endregion variables

        #region properties

        public QuestionData[] QuestionsPoolFromCatagory { get; set; }

        public QuestionController QuestionController
        {
            get
            {
                if (QuestionController.Initialised)
                    return QuestionController.Instance;
                else return null;
            }
        }

        public DataController DataController
        {
            get
            {
                if (DataController.Initialised)
                    return DataController.Instance;
                else return null;
            }
        }

        #endregion properties

        #region methods

        #region unity

        protected override void OnEnable()
        {
            base.OnEnable();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _checkForOpenGames = FindObjectOfType<CheckForOpenGames>();
            _menuController = FindObjectOfType<MenuController>();
            _checkForPlayerExistingGames = FindObjectOfType<CheckForPlayerExistingGames>();

            Debug.Log("[GameLobbyController] Start() : Here we go");

			if (PlayerPrefs.HasKey((DataHelper.PlayerDataKey.GAMEKEY) + PlayerController.Instance.GetUsername()))
            {
				Debug.Log("[GameLobbyController] Start() : Found player: " + PlayerPrefs.GetString(DataHelper.PlayerDataKey.GAMEKEY) + PlayerController.Instance.GetUsername());
                _checkForPlayerExistingGames.GetPlayersOpenGames();
            }
            else
                Debug.Log("[GameLobbyController] Start() : Player has no ongoing games");
        }

        #endregion unity

        #region GameLobbyController specific

        public void StartOpenGame(OngoingGamesData _ongoingGameData)
        {
            Debug.Log(_ongoingGameData.askedQuestions);
        }

        public void StartNewGame()
        {
            Debug.Log("[GameLobbyController] StartNewGame() : Checking for open games");
            _checkForOpenGames.CheckForGamesNeedingOpponents();
        }

        public void BackToMenu()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        public void PresentPopUp()
        {
            CatagoryPopUp.SetActive(true);
            CatagoryDropDown.gameObject.SetActive(true);
            BackgroundStuff.SetActive(false);

            if (DataController.TurnNumber == 1 || DataController.TurnNumber == 3)
            {
                CatSelectPanel.SetActive(true);
                PopulateDropDown();
            }

            if (DataController.TurnNumber == 2)
            {
                CatAckPanel.SetActive(true);
                Debug.Log("[GameLobbyController] PresentPopUp() : Round catagory is : " + DataController.OngoingGameData.Round1Catagory);
                CatagoryText.text = DataController.OngoingGameData.Round1Catagory;
            }

            if (DataController.Instance.TurnNumber == 4)
            {
                CatAckPanel.SetActive(true);
                Debug.Log("[GameLobbyController] PresentPopUp() : Round catagory is : " + DataController.OngoingGameData.Round2Catagory);
                CatagoryText.text = DataController.OngoingGameData.Round2Catagory;
            }

            if (DataController.TurnNumber == 5)
            {
                CatAckPanel.SetActive(true);
                string randomCatagory = "";
                randomCatagory = QuestionController.GetRandomCatagory();
                QuestionsPoolFromCatagory = QuestionController.GetQuestionsInCatagory(randomCatagory);

                Debug.Log("[GameLobbyController] PresentPopUp() : Round catagory is : " + randomCatagory);

                CatagoryText.text = randomCatagory;
            }

            if (DataController.TurnNumber == 6)
            {
                CatAckPanel.SetActive(true);
                Debug.Log("[GameLobbyController] PresentPopUp() : Round catagory is :  " + DataController.OngoingGameData.round3Cat);
                CatagoryText.text = DataController.OngoingGameData.round3Cat;
            }
        }

        private void PopulateDropDown()
        {
            _catagoryList = QuestionController.GetAllCategories();

            if (DataController.TurnNumber == 3)
                _catagoryList = QuestionController.RemoveCatagory(_catagoryList, DataController.Instance.OngoingGameData.Round1Catagory);

            CatagoryDropDown.AddOptions(_catagoryList);
        }

        public void CatagorySelected()
        {
            string catagory = (CatagoryDropDown.options[CatagoryDropDown.value]).text;
            Debug.Log("[GameLobbyController] CatagorySelected() : Catagory selected: " + catagory);
            QuestionsPoolFromCatagory = QuestionController.GetQuestionsInCatagory(catagory);
            DataController.Catagory = catagory;
            _menuController.StartGame();
        }

        public void catagoryAcknowledged()
        {
            _menuController.StartGame();
        }
    }

    #endregion GameLobbyController specific

    #endregion methods
}