using System.Collections;
using UnityEngine;

namespace _LetsQuiz
{
    public class SubmitGame : MonoBehaviour
    {
        private PlayerController _playerController;
        private QuestionController _questionController;
        private DataController _dataController;
        private string _questionPool;
        private int _counter; //just used for testing, so the log can show what was submitted
        private bool ConnectionAvailable;

        // Use this for initialization
        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _dataController = FindObjectOfType<DataController>();
            _questionController = FindObjectOfType<QuestionController>();
        }

        public void SubmitGameToDB(string _questionPool)
        {
            checkForConnection();  //test for network connectivity
            uploadExistingGames();                      //test for existing games to be uploaded
            this._questionPool = _questionPool;
            StartCoroutine(SubmitRoundData());
        }

        private IEnumerator SubmitRoundData()
        {
            string address = "";
            WWWForm form = new WWWForm();
            if (_dataController.TurnNumber == 1)
            {
                form.AddField("gameNumberPost", _dataController.GameNumber); //TODO need a better way to generate unique game numbers for the first game
                form.AddField("playerNamePost", _playerController.GetUsername());
                form.AddField("askedQuestionsPost", _questionController.GetAskedQuestions());
                form.AddField("QuestionsLeftInCatagoryPost", _questionPool);
                form.AddField("Round1CatagoryPost", _dataController.Catagory.ToString());
                form.AddField("scorePost", _playerController.UserScore);
                form.AddField("turnsCompletedPost", _dataController.TurnNumber);

                Debug.Log(_playerController.GetId().ToString() + " " + _playerController.GetGamesPlayed().ToString() + " " + _playerController.GetTotalQuestionsAnswered().ToString() + " " + _playerController.GetNumberCorrectAnswers().ToString());
                form.AddField("userIDPost", _playerController.GetId().ToString());
                form.AddField("totalGamesPlayedPost", _playerController.GetGamesPlayed().ToString());
                form.AddField("totalQuestionsPost", _playerController.GetTotalQuestionsAnswered().ToString());
                form.AddField("totalCorrectQuestionsPost", _playerController.GetNumberCorrectAnswers().ToString());

                _counter = 1;

                address = ServerHelper.Host + ServerHelper.SubmitRound1Data;
            }

            if (_dataController.TurnNumber == 2)
            {
                Debug.Log("Submitting round 2 data");
                form.AddField("gameNumberPost", _dataController.OngoingGameData.gameNumber); //TODO need a better way to generate unique game numbers for the first game
                form.AddField("askedQuestionsPost", "nothing");
                form.AddField("opponentNamePost", _playerController.GetUsername());
                form.AddField("scorePost", _playerController.UserScore);
                form.AddField("gameRequiresOppoentPost", 0);
                form.AddField("turnsCompletedPost", _dataController.TurnNumber);
                form.AddField("overAllScorePost", _dataController.getOverAllScore());

                Debug.Log(_playerController.GetId().ToString() + " " + _playerController.GetGamesPlayed().ToString() + " " + _playerController.GetTotalQuestionsAnswered().ToString() + " " + _playerController.GetNumberCorrectAnswers().ToString());
                form.AddField("userIDPost", _playerController.GetId().ToString());
                form.AddField("totalGamesPlayedPost", _playerController.GetGamesPlayed().ToString());
                form.AddField("totalQuestionsPost", _playerController.GetTotalQuestionsAnswered().ToString());
                form.AddField("totalCorrectQuestionsPost", _playerController.GetNumberCorrectAnswers().ToString());

                _counter = 2;

                address = ServerHelper.Host + ServerHelper.SubmitRound2Data;
            }

            if (_dataController.TurnNumber == 3)
            {
                Debug.Log("Submitting round 3 data");
                form.AddField("gameNumberPost", _dataController.OngoingGameData.gameNumber);
                form.AddField("askedQuestionsPost", _questionController.GetAskedQuestions());
                form.AddField("Round2CatagoryPost", _dataController.Catagory.ToString());
                form.AddField("QuestionsLeftInCatagoryPost", _questionPool);
                form.AddField("scorePost", _playerController.UserScore);
                form.AddField("turnsCompletedPost", _dataController.TurnNumber);

                Debug.Log(_playerController.GetId().ToString() + " " + _playerController.GetGamesPlayed().ToString() + " " + _playerController.GetTotalQuestionsAnswered().ToString() + " " + _playerController.GetNumberCorrectAnswers().ToString());
                form.AddField("userIDPost", _playerController.GetId().ToString());
                form.AddField("totalGamesPlayedPost", _playerController.GetGamesPlayed().ToString());
                form.AddField("totalQuestionsPost", _playerController.GetTotalQuestionsAnswered().ToString());
                form.AddField("totalCorrectQuestionsPost", _playerController.GetNumberCorrectAnswers().ToString());

                _counter = 3;

                address = ServerHelper.Host + ServerHelper.SubmitRound3Data;
            }
            if (_dataController.TurnNumber == 4)
            {
                Debug.Log("Submitting round 4 data");
                form.AddField("gameNumberPost", _dataController.OngoingGameData.gameNumber);
                form.AddField("scorePost", _playerController.UserScore);
                form.AddField("turnsCompletedPost", _dataController.TurnNumber);
                form.AddField("overAllScorePost", _dataController.getOverAllScore());

                _counter = 4;

                address = ServerHelper.Host + ServerHelper.SubmitRound4Data;
            }
            if (_dataController.TurnNumber == 5)
            {
                Debug.Log("Submitting round 5 data");
                form.AddField("gameNumberPost", _dataController.OngoingGameData.gameNumber);
                form.AddField("askedQuestionsPost", _questionController.GetAskedQuestions());
                form.AddField("Round3CatagoryPost", _dataController.Catagory.ToString());
                form.AddField("QuestionsLeftInCatagoryPost", _questionPool);
                form.AddField("scorePost", _playerController.UserScore);
                form.AddField("turnsCompletedPost", _dataController.TurnNumber);

                _counter = 5;

                address = ServerHelper.Host + ServerHelper.SubmitRound5Data;
            }

            if (_dataController.TurnNumber == 6)
            {
                form.AddField("gameNumberPost", _dataController.OngoingGameData.gameNumber);
                address = ServerHelper.Host + ServerHelper.SubmitRound6Data;
                _dataController.OngoingGameData.opponentScore = +_playerController.UserScore;

                _counter = 6;
            }

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
                    _playerController.AddSavedGame(new SavedGame(submitRequest));
                    yield return null;
                }

                // extra check just to ensure a stream error doesn't come up
                if (_connectionTimer > _connectionTimeLimit || submitRequest.error != null)
                {
                    FeedbackAlert.Show("Server error.");
                    Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                    _playerController.AddSavedGame(new SavedGame(submitRequest));
                    yield return null;
                }
            }

            if (submitRequest.error != null)
            {
                FeedbackAlert.Show("Connection error. Please try again.");
                Debug.Log("SubmitScore : Submit() : " + submitRequest.error);
                _playerController.AddSavedGame(new SavedGame(submitRequest));
                yield return null;
            }

            if (submitRequest.isDone)
            {
                Debug.Log("game data submitted, using data #" + _counter);
                yield return submitRequest;
                DestroyObject(gameObject);
            }
        }

        public void checkForConnection()
        {
            //testing for network connectivity
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    ConnectionAvailable = false;
                    break;

                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    ConnectionAvailable = true;
                    break;

                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    ConnectionAvailable = true;
                    break;
            }
            //how you check if a connection is available
            if (ConnectionAvailable)
            {
                Debug.Log("Connection was succcessful");
            }
            else
            {
                Debug.Log("Connection was not succcessful");
            }
        }

        //upload any exisitng games stored in the player files
        public void uploadExistingGames()
        {
            Debug.Log("Attempting to upload Existing Games");

            if (_playerController.GetSavedGames() != null)
            {
                SavedGameContainer games = _playerController.GetSavedGames();
                //iterate through length of saved games
                for (int i = 0; i < games.AllSavedRounds.Count; i++)
                {
                    //for each game, attempt to upload.
                    //if successful then remove from list

                    WWW submitRequest = games.AllSavedRounds[i]._submitRequest;
                    while (!submitRequest.isDone)
                    {
                        float _connectionTimer = 0.0f;
                        const float _connectionTimeLimit = 1000000.0f;
                        _connectionTimer += Time.deltaTime;
                        if (_connectionTimer > _connectionTimeLimit)
                        {
                            FeedbackAlert.Show("Server time out.");
                            Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                            break;
                        }

                        // extra check just to ensure a stream error doesn't come up
                        if (_connectionTimer > _connectionTimeLimit || submitRequest.error != null)
                        {
                            FeedbackAlert.Show("Server error.");
                            Debug.LogError("SubmitScore : Submit() : " + submitRequest.error);
                            break;
                        }
                    }

                    if (submitRequest.error != null)
                    {
                        FeedbackAlert.Show("Connection error. Please try again.");
                        Debug.Log("SubmitScore : Submit() : " + submitRequest.error);
                        break;
                    }

                    if (submitRequest.isDone)
                    {
                        Debug.Log("game data submitted, using data #" + _counter);
                        DestroyObject(gameObject);
                        games.AllSavedRounds.RemoveAt(i);  //remove from current list
                    }
                }

                //resave games List to player prefs, any outstanding uploads will be saved back.
                _playerController.SetSavedGames(games);
            }
            else
            {
                Debug.Log("No outstanding games to upload");
            }
        }
    }
}