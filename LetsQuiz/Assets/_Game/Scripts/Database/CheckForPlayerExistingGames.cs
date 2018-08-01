using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class CheckForPlayerExistingGames : MonoBehaviour
    {
        #region variables

        private float _downloadTimer = 5.0f;

        private DataController _dataController;
        private PlayerController _playerController;
        private GameLobbyController _GameLobbyController;
        private MenuController _MenuController;
        private string openGamesJSON;
        private bool _isInteractable = false;
        //public bool continueExistingGame { get; set; }
        //private OngoingGamesData _OngoingGamesData;

        public GameObject gameButton;
        public Transform buttonContainer;

        #endregion variables

        #region methods

        #region unity

        private void Begin()
        {
            _dataController = FindObjectOfType<DataController>();
            _playerController = FindObjectOfType<PlayerController>();
            _MenuController = FindObjectOfType<MenuController>();
        }

        #endregion unity

        #region download specific

        public void GetPlayersOpenGames()
        {
            Begin();
            StartCoroutine(PlayersOpenGames());
        }

        private IEnumerator PlayersOpenGames()
        {
            Debug.Log("checking players open games");
            if (!PlayerPrefs.HasKey(_playerController.GetUsername()))
                Debug.Log("player has no open games in playerprefs");
            if (PlayerPrefs.HasKey(_playerController.GetUsername()))
                Debug.Log("player has open games: " + PlayerPrefs.GetString(_playerController.GetUsername()));

            WWWForm form = new WWWForm();
            form.AddField("gameNumbersPost", PlayerPrefs.GetString(_playerController.GetUsername())); //TODO need a better way to generate unique game numbers for the first game
            string address = ServerHelper.Host + ServerHelper.GetPlayersOpenGames;
            WWW submitRequest = new WWW(address, form);
            while (!submitRequest.isDone)
            {
                float _connectionTimer = 0.0f;
                const float _connectionTimeLimit = 1000000.0f;
                _connectionTimer += Time.deltaTime;
                if (_connectionTimer > _connectionTimeLimit)
                {
                    FeedbackAlert.Show("Server time out.");
                    Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                    yield return null;
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || submitRequest.error != null)
                {
                    FeedbackAlert.Show("Server error.");
                    Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                    yield return null;
                }
            }

            if (submitRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.Log("SubmitScore : Submit() : " + submitRequest.error);
                yield return null;
            }

            if (submitRequest.isDone)
            {
                Debug.Log("got open game data");
                Debug.Log(submitRequest.text);
                displayOpenGames(submitRequest.text);
                yield return submitRequest;
            }
        }

        public void displayOpenGames(string openGamesStartedByTheUser)
        {
            openGamesStartedByTheUser = "{\"dataForOpenGame\":" + openGamesStartedByTheUser + "}";
            Debug.Log("JSON of playersOngoingGames " + openGamesStartedByTheUser);
            OngoingGamesContainer gamesPlayerHasStarted = new OngoingGamesContainer();
            gamesPlayerHasStarted = JsonUtility.FromJson<OngoingGamesContainer>(openGamesStartedByTheUser); //serialize opengame data

            if (gamesPlayerHasStarted.dataForOpenGame.Length == 0)
                Debug.Log("user has no open games");

            for (int i = 0; i < gamesPlayerHasStarted.dataForOpenGame.Length; i++)
            {
                GameObject go = new GameObject();

                //bool isInteractable = isButtonInteractable (gamesPlayerHasStarted, i, playerOne, img);

                go = Instantiate(gameButton) as GameObject; //Instantiate must be after you set all the variables on that object!
                go.transform.SetParent(buttonContainer);

                OngoingGamesData gameData = gamesPlayerHasStarted.dataForOpenGame[i];
                go.GetComponentInChildren<Button>().onClick.AddListener(() => continueGameButtonPressed(gameData));
                go.GetComponentInChildren<Text>().text = gameData.gameNumber.ToString(); //this is what is written on each button

                isInteractable(gameData, go);
                go.transform.localScale = new Vector3(1, 1, 1); // the scale on my prefab is blowing out at runtime, this fixes that problem
            }
        }

        private void continueGameButtonPressed(OngoingGamesData gameData)
        {
            Debug.Log(gameData.gameNumber);
            _dataController.ongoingGameData = gameData;
            _dataController.turnNumber = _dataController.ongoingGameData.turnNumber;
            _dataController.turnNumber++;
            _MenuController.StartGame();
        }

        private void isInteractable(OngoingGamesData gameData, GameObject go)
        {
            Button b;
            var colors = go.GetComponentInChildren<Button>().colors;
            if (_playerController.GetUsername() == gameData.player)
            {
                if (gameData.turnNumber == 1 || gameData.turnNumber == 2 || gameData.turnNumber == 5)
                {
                    b = go.GetComponentInChildren<Button>();
                    b.GetComponent<Image>().color = Color.red;
                    go.GetComponentInChildren<Button>().interactable = false;
                }
                if (gameData.turnNumber == 3 || gameData.turnNumber == 4)
                {
                    b = go.GetComponentInChildren<Button>();
                    b.GetComponent<Image>().color = Color.green;
                    go.transform.SetAsFirstSibling();
                    go.GetComponentInChildren<Button>().interactable = true;
                }
            }
            if (_playerController.GetUsername() == gameData.opponent)
            {
                if (gameData.turnNumber == 1)
                    Debug.LogError("this should not happen");
                if (gameData.turnNumber == 3 || gameData.turnNumber == 4)
                {
                    b = go.GetComponentInChildren<Button>();
                    b.GetComponent<Image>().color = Color.red;
                    go.GetComponentInChildren<Button>().interactable = false;
                }

                if (gameData.turnNumber == 2 || gameData.turnNumber == 5)
                {
                    b = go.GetComponentInChildren<Button>();
                    b.GetComponent<Image>().color = Color.green;
                    go.GetComponentInChildren<Button>().interactable = true;
                }
            }
        }
    }

    #endregion download specific

    #endregion methods
}