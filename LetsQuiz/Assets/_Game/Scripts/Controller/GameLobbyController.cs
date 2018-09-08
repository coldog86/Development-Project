using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", BackToMenu, FeedbackTwoButtonModal.Hide);
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

            if (DataController.Instance.TurnNumber == 1 || DataController.Instance.TurnNumber == 3)
            {
                CatSelectPanel.SetActive(true);
                PopulateDropDown();
            }

            if (DataController.Instance.TurnNumber == 2)
            {
                CatAckPanel.SetActive(true);
                Debug.Log("[GameLobbyController] PresentPopUp() : Round catagory is : " + DataController.Instance.OngoingGameData.Round1Catagory);
                CatagoryText.text = DataController.Instance.OngoingGameData.Round1Catagory;
            }

            if (DataController.Instance.TurnNumber == 4)
            {
                CatAckPanel.SetActive(true);
                Debug.Log("[GameLobbyController] PresentPopUp() : Round catagory is : " + DataController.Instance.OngoingGameData.Round2Catagory);
                CatagoryText.text = DataController.Instance.OngoingGameData.Round2Catagory;
            }

            if (DataController.Instance.TurnNumber == 5)
            {
                CatAckPanel.SetActive(true);
                string randomCatagory = "";
                randomCatagory = QuestionController.Instance.GetRandomCatagory();
                QuestionsPoolFromCatagory = QuestionController.Instance.GetQuestionsInCatagory(randomCatagory);

                Debug.Log("[GameLobbyController] PresentPopUp() : Round catagory is : " + randomCatagory);

                CatagoryText.text = randomCatagory;
            }

            if (DataController.Instance.TurnNumber == 6)
            {
                CatAckPanel.SetActive(true);
                Debug.Log("[GameLobbyController] PresentPopUp() : Round catagory is :  " + DataController.Instance.OngoingGameData.round3Cat);
                CatagoryText.text = DataController.Instance.OngoingGameData.round3Cat;
            }
        }

        private void PopulateDropDown()
        {
            _catagoryList = QuestionController.Instance.GetAllCategories();
			_catagoryList.Insert (0, "Random Catagory");

            if (DataController.Instance.TurnNumber == 3)
                _catagoryList = QuestionController.Instance.RemoveCatagory(_catagoryList, DataController.Instance.OngoingGameData.Round1Catagory);

            CatagoryDropDown.AddOptions(_catagoryList);
        }

        public void CatagorySelected()
        {
            string catagory = (CatagoryDropDown.options[CatagoryDropDown.value]).text;
            Debug.Log("[GameLobbyController] CatagorySelected() : Catagory selected: " + catagory);

			_catagoryList = QuestionController.Instance.GetAllCategories();

			if (catagory == "Random Catagory") 
			{
				int randomNumber = Random.Range(0, _catagoryList.Count - 1); //gets random number between 0 and total number of catagories
				catagory = _catagoryList[randomNumber];
			}
			QuestionsPoolFromCatagory = QuestionController.Instance.GetQuestionsInCatagory(catagory);
            DataController.Instance.Catagory = catagory;
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