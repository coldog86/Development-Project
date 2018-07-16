using System.Collections;
using UnityEngine;

namespace _LetsQuiz
{
    public class CheckForOpenGames : MonoBehaviour
    {
        #region variables

        private float _downloadTimer = 5.0f;

        private DataController _dataController;
        private PlayerController _playerController;
        private GameLobbyController _GameLobbyController;
        private string openGamesJSON;
		//public bool continueExistingGame { get; set; }
		//private OngoingGamesData _OngoingGamesData;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            _dataController = FindObjectOfType<DataController>();
            _playerController = FindObjectOfType<PlayerController>();

        }

        #endregion

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
                    _dataController.serverConnected = false;
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
                _dataController.serverConnected = false;               
            }
            else
            { 
                // we got the string from the server, it is every question in JSON format
                Debug.Log("Vox transmition recieved");
                Debug.Log(download.text);

                _dataController.serverConnected = true;
                openGamesJSON = download.text;

                handleData();
                yield return download;


            } 
        }

        public void handleData ()
		{
			OngoingGamesContainer og = new OngoingGamesContainer ();
			openGamesJSON = "{\"dataForOpenGame\":" + openGamesJSON + "}"; 
			og = JsonUtility.FromJson<OngoingGamesContainer> (openGamesJSON);

			int n = -1;
			for (int i = 0; i < og.dataForOpenGame.Length; i++) { //check the current user did not start the open game
				if (og.dataForOpenGame [i].player != _playerController.GetUsername ())
					n = i;
			}
			if (n < 0) {
				Debug.Log("no open games - turn = 1");
				//continueExistingGame = false;
				_dataController.turnNumber = 1;
			}

			if (n > -1) { 
				Debug.Log("there are open game(s) - turn = 2");
				//continueExistingGame = true;
				_dataController.turnNumber = 2;
				//OngoingGamesData temp = og.dataForOpenGame[n];
				//_GameLobbyController = FindObjectOfType<GameLobbyController>();
				//_GameLobbyController.StartOpenGame(temp);
				Debug.Log("****asked questions = " + og.dataForOpenGame[n].askedQuestions);
				Debug.Log("****remaining questions = " + og.dataForOpenGame[n].QuestionsLeftInCatagory);

				_dataController.ongoingGameData = og.dataForOpenGame[n];

			
			}
				
        }

        #endregion

        #endregion

    }
}

