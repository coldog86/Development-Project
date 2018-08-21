using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _LetsQuiz
{
    public class CheckForOpenGames : MonoBehaviour
    {
        #region variables

        private float _downloadTimer = 5.0f;
        private string _openGamesJSON;

        #endregion variables

        #region properties

        public DataController DataController
        {
            get
            {
                if (DataController.Initialised)
                    return DataController.Instance;
                else return null;
            }
        }

        public PlayerController PlayerController
        {
            get
            {
                if (PlayerController.Initialised)
                    return PlayerController.Instance;
                else return null;
            }
        }

        public GameLobbyController GameLobbyController
        {
            get
            {
                if (GameLobbyController.Initialised)
                    return GameLobbyController.Instance;
                else return null;
            }
        }

        #endregion properties

        #region methods

        #region download specific

        public void CheckForGamesNeedingOpponents()
        {
            StartCoroutine(CheckForGamesNeedingOpponent());
        }

        private IEnumerator CheckForGamesNeedingOpponent()
        {
            WWW download = new WWW(ServerHelper.Host + ServerHelper.CheckForOpenGames);
            while (!download.isDone)
            {
                if (_downloadTimer < 0)
                {
                    Debug.LogError("Server time out.");
                    DataController.ServerConnected = false;
                    break;
                }
                _downloadTimer -= Time.deltaTime;
                yield return null;
            }

            if (!download.isDone || download.error != null)
            {
                /* TODO if we cannot connect to the server or there is some error in the data,
                 * check the prefs for previously saved questions */
                Debug.LogError(download.error);
                Debug.Log("Failed to hit the server.");
                DataController.ServerConnected = false;
            }
            else
            {
                // we got the string from the server, it is every question in JSON format
                Debug.Log("Vox transmition recieved");
                Debug.Log(download.text);

                DataController.ServerConnected = true;
                _openGamesJSON = download.text;

                handleData();
                yield return download;
            }
        }

        public void handleData()
        {
            OngoingGamesContainer og = new OngoingGamesContainer();
            _openGamesJSON = "{\"dataForOpenGame\":" + _openGamesJSON + "}";
            og = JsonUtility.FromJson<OngoingGamesContainer>(_openGamesJSON);

            int n = -1;
            for (int i = 0; i < og.dataForOpenGame.Length; i++)
            { //check the current user did not start the open game
                if (og.dataForOpenGame[i].player != PlayerController.GetUsername())
                    n = i;
            }
            if (n < 0)
            {
                Debug.Log("no open games - turn = 1");
                //continueExistingGame = false;
                DataController.TurnNumber = 1;
                int rand = Random.Range(1, 100000);
                Debug.Log("game number = " + rand);
                DataController.GameNumber = rand;
            }

            if (n > -1)
            {
                Debug.Log("there are open game(s) - turn = 2");
                DataController.TurnNumber = 2;
                Debug.Log("****asked questions = " + og.dataForOpenGame[n].askedQuestions);
                Debug.Log("****remaining questions = " + og.dataForOpenGame[n].questionsLeftInCat);

                if (PlayerPrefs.HasKey(DataHelper.PlayerDataKey.USERNAME))
                {
                    string games = PlayerPrefs.GetString(DataHelper.PlayerDataKey.USERNAME);
                    games = games + "," + DataController.GameNumber.ToString();
                    PlayerPrefs.SetString(DataHelper.PlayerDataKey.USERNAME, games);
                    Debug.Log("games in player prefs = " + PlayerPrefs.GetString(DataHelper.PlayerDataKey.USERNAME));
                }
                if (!PlayerPrefs.HasKey(PlayerController.GetUsername()))
                {
                    PlayerPrefs.SetString(DataHelper.PlayerDataKey.USERNAME, DataController.GameNumber.ToString());
                    Debug.Log("games in player prefs = " + PlayerPrefs.GetString(DataHelper.PlayerDataKey.USERNAME));
                }

                DataController.OngoingGameData = og.dataForOpenGame[n];
                DataController.GameNumber = og.dataForOpenGame[n].gameNumber;
            }
            Debug.Log("turn number = " + DataController.TurnNumber);
            GameLobbyController.PresentPopUp();
        }

        #endregion download specific

        #endregion methods
    }
}