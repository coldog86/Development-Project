using UnityEngine;
using System.Collections.Generic;

namespace _LetsQuiz
{
    public class PlayerController : MonoBehaviour
    {
        #region variables

        private const string _typeKey = "PlayerType";
        private const string _idKey = "PlayerId";
        private const string _usernameKey = "PlayerUsername";
        private const string _emailKey = "PlayerEmail";
        private const string _passwordKey = "PlayerPassword";
        private const string _dobKey = "PlayerDOB";
        private const string _questionsSubmittedKey = "PlayerQuestionsSubmitted";
        private const string _numberQuestionsSubmittedKey = "PlayerNumberQuestionsSubmitted";
        private const string _numberGamesPlayedKey = "PlayerNumberGamesPlayed";
        private const string _highestScoreKey = "PlayerHighestScore";
        private const string _numberCorrectAnswersKey = "PlayerNumberCorrectAnswers";
        private const string _totalQuestionsAnsweredKey = "PlayerTotalQuestionsAnswered";
        private const string _questionDataKey = "PlayerQuestionData";
        private const string _ongoingGamesKey = "OngoingGamesKey";

        [Header("Player Details")]
        [SerializeField]
        private Player _player;

        [Header("Player Status")]
        [SerializeField]
        private int _type = 0;

        [Header("Player Content")]
        [SerializeField]
        private string _questionData = "";
        private string _highScoreData = "";
        private QuestAndSub[] _questandSub;

        private DataController _dataController;

        #endregion variables

        #region properties

        public string ongoingGamesKey { get { return _ongoingGamesKey; } }

        public string typeKey { get { return _typeKey; } }

        public string usernameKey { get { return _usernameKey; } }

        public string idKey { get { return _idKey; } }

        public string emailKey { get { return _emailKey; } }

        public string passwordKey { get { return _passwordKey; } }

        public string dobKey { get { return _dobKey; } }

        public string questionsSubmittedKey { get { return _questionsSubmittedKey; } }

        public string numberQuestionsSubmittedKey { get { return _numberQuestionsSubmittedKey; } }

        public string numberGamesPlayedKey { get { return _numberGamesPlayedKey; } }

        public string highestScoreKey { get { return _highestScoreKey; } }

        public string numberCorrectAnswersKey { get { return _numberCorrectAnswersKey; } }

        public string totalQuestionsAnsweredKey { get { return _totalQuestionsAnsweredKey; } }

        public string questionDataKey { get { return _questionDataKey; } }

        public string highScoreJSON { get; set; }

        public int userScore { get; set; }

        public string scoreStatus { get; set; }

		public List<SavedGame> savedGames { get; set; }

        #endregion properties

        #region id

        private void Start()
        {
            _dataController = FindObjectOfType<DataController>();

			Debug.Log ("current saved games");
			Debug.Log (savedGames);
        }

        // set the player id value
        public void SetId(int id)
        {
            if (id != _player.ID)
            {
                _player.ID = id;
                SaveId();
            }
        }

        // get the player id value
        public int GetId()
        {
            return _player.ID;
        }

        // save the player id value in playerprefs
        private void SaveId()
        {
            PlayerPrefs.SetInt(_idKey, _player.ID);
            PlayerPrefs.Save();
        }

        #endregion id

        #region status

        // set the player status value
        public void SetPlayerType(int type)
        {
            if (type != _type)
            {
                _type = type;
                SavePlayerType();
            }
        }

        // get the player status value
        public int GetPlayerType()
        {
            return _type;
        }

        // save the player status value in playerprefs
        private void SavePlayerType()
        {
            PlayerPrefs.SetInt(_typeKey, _type);
            PlayerPrefs.Save();
        }

        #endregion status

        #region username

        // set the player username value
        public void SetUsername(string username)
        {
            if (username != _player.username)
            {
                _player.username = username;
                SaveUsername();
            }
        }

        // get the player username value
        public string GetUsername()
        {
            return _player.username;
        }

        // save the player username value in playerprefs
        private void SaveUsername()
        {
            PlayerPrefs.SetString(_usernameKey, _player.username);
            PlayerPrefs.Save();
        }

        #endregion username

        #region email

        // set the player email value
        public void SetEmail(string email)
        {
            if (email != _player.email)
            {
                _player.email = email;
                SaveEmail();
            }
        }

        // get the player email value
        public string GetEmail()
        {
            return _player.email;
        }

        // save the player email value in playerprefs
        private void SaveEmail()
        {
            PlayerPrefs.SetString(_emailKey, _player.email);
            PlayerPrefs.Save();
        }

        #endregion email

        #region password

        // set the player password value
        public void SetPassword(string password)
        {
            if (password != _player.password)
            {
                _player.password = password;
                SavePassword();
            }
        }

        // get the player password value
        public string GetPassword()
        {
            return _player.password;
        }

        // save the player password value in playerprefs
        private void SavePassword()
        {
            PlayerPrefs.SetString(_passwordKey, _player.password);
            PlayerPrefs.Save();
        }

        #endregion password

        #region dob

        // set the player dob value
        public void SetDOB(string date)
        {
            if (date != _player.DOB)
            {
                _player.DOB = date;
                SaveDOB();
            }
        }

        // get the player dob value
        public string GetDOB()
        {
            return _player.DOB;
        }

        // save the player dob value in playerprefs
        private void SaveDOB()
        {
            PlayerPrefs.SetString(_dobKey, _player.DOB);
            PlayerPrefs.Save();
        }

        #endregion dob

        #region questions submitted

        // set the player questions submitted value
        public void SetQuestionsSubmitted(string questions)
        {
            if (questions != _player.questionsSubmitted)
            {
                _player.questionsSubmitted = questions;
                SaveQuestionsSubmitted();
            }
        }

        // get the player questions submitted value
        public string GetQuestionsSubmitted()
        {
            return _player.questionsSubmitted;
        }

        // save the player questions submitted value in playerprefs
        private void SaveQuestionsSubmitted()
        {
            PlayerPrefs.SetString(_questionsSubmittedKey, _player.questionsSubmitted);
            PlayerPrefs.Save();
        }

        #endregion questions submitted

        #region number questions submitted

        // set the player number questions submitted value
        public void SetNumberQuestionsSubmitted(int questionsSubmitted)
        {
            if (questionsSubmitted != _player.numQuestionsSubmitted)
            {
                _player.numQuestionsSubmitted = questionsSubmitted;
                SaveNumberQuestionsSubmitted();
            }
        }

        // get the player number questions submitted value
        public int GetNumberQuestionsSubmitted()
        {
            return _player.numQuestionsSubmitted;
        }

        // save the player number questions submitted value in playerprefs
        private void SaveNumberQuestionsSubmitted()
        {
            PlayerPrefs.SetInt(_numberQuestionsSubmittedKey, _player.numQuestionsSubmitted);
            PlayerPrefs.Save();
        }

        #endregion number questions submitted

        #region number games played

        // set the player games played value
        public void SetGamesPlayed(int gamesPlayed)
        {
            if (gamesPlayed != _player.numGamesPlayed)
            {
                SaveGamesPlayed(gamesPlayed);
            }
        }

        public void AddToGamesPlayed()
        {
            _player.numGamesPlayed = _player.numGamesPlayed++;
            Debug.Log("games played: " + _player.numGamesPlayed);
            SaveGamesPlayed(_player.numGamesPlayed);
        }

        // get the player games played value
        public int GetGamesPlayed()
        {
            return _player.numGamesPlayed;
        }

        // save the player games played value in playerprefs
        private void SaveGamesPlayed(int num)
        {
            _player.numGamesPlayed = num;
            PlayerPrefs.SetInt(_numberGamesPlayedKey, _player.numGamesPlayed);
            PlayerPrefs.Save();
        }

        #endregion number games played

        #region highest score

        // set the player highest score value
        public void SetHighestScore(int score)
        {
            _player.HighestScore = score;
        }

        // get the player number questions submitted value
        public int GetHighestScore()
        {
            return _player.HighestScore;
        }

        // save the player number questions submitted value in playerprefs
        private void SaveHighestScore()
        {
            PlayerPrefs.SetInt(_highestScoreKey, _player.HighestScore);
            PlayerPrefs.Save();
        }

        #endregion highest score

        #region number correct answers

        // set the player correct answers value

        /*
		public void SetNumberCorrectAnswers(int answers)
        {
            if (answers > _player.numCorrectAnswers)
            {
                _player.numCorrectAnswers = answers;
				SaveNumberCorrectAnswers(answers);
            }
        }
*/

        public void AddToNumberCorrectAnswers()
        {
            _player.TotalCorrectAnswers = _player.TotalCorrectAnswers++;
            SaveNumberCorrectAnswers(_player.TotalCorrectAnswers);
        }

        // get the player correct answers value
        public int GetNumberCorrectAnswers()
        {
            return _player.TotalCorrectAnswers;
        }

        // save the player correct answers value in playerprefs
        private void SaveNumberCorrectAnswers(int number)
        {
            _player.TotalCorrectAnswers = number;
            PlayerPrefs.SetInt(_numberCorrectAnswersKey, _player.TotalCorrectAnswers);
            PlayerPrefs.Save();
        }

        #endregion number correct answers

        #region total questions answered

        // set the player number questions answered value
        public void SetTotalQuestionsAnswered(int questionsAnswered)
        {
            if (questionsAnswered > _player.totalQuestionsAnswered)
            {
                _player.totalQuestionsAnswered = questionsAnswered;
                SaveTotalQuestionsAnswered(questionsAnswered);
            }
        }

        public void AddToTotalQuestionsAnswered()
        {
            _player.totalQuestionsAnswered = _player.totalQuestionsAnswered++;
            SaveTotalQuestionsAnswered(_player.totalQuestionsAnswered);
        }

        // get the player number questions answered value
        public int GetTotalQuestionsAnswered()
        {
            return _player.totalQuestionsAnswered;
        }

        // save the player number questions answered value in playerprefs
        private void SaveTotalQuestionsAnswered(int total)
        {
            _player.totalQuestionsAnswered = total;
            PlayerPrefs.SetInt(_totalQuestionsAnsweredKey, _player.totalQuestionsAnswered);
            PlayerPrefs.Save();
        }

        #endregion total questions answered

        #region question data

        // set the player question data
        public void SetQuestionData(string questionData)
        {
            if (questionData != _questionData)
            {
                _questionData = questionData;
                SaveQuestionData();
            }
        }

        // get the player question data
        public string GetQuestionData()
        {
            return _questionData;
        }

        // save the player question data in playerprefs
        private void SaveQuestionData()
        {
            PlayerPrefs.SetString(_questionDataKey, _questionData);
            PlayerPrefs.Save();
        }

        // set the worldwide highscore data
        public void SetHighscoreData(string highScoreData)
        {
            if (highScoreData != highScoreJSON)
            {
                highScoreJSON = highScoreData;
            }
        }

        // get worldwide highscore data
        public string GetHighScoreData()
        {
            return _highScoreData;
        }

        //set questandSub
        public void SetQuestandSubData(QuestAndSub[] questandSub)
        {
            _questandSub = questandSub;
        }

        //get questandSub
        public QuestAndSub[] GetQuestandSubData()
        {
            return _questandSub;
        }

		//get all saved games
		public List<SavedGame> getSavedGames()
		{
			return savedGames;
		}

		//add to saved games
		public void addSavedGame(SavedGame game)
		{
			savedGames.Add (game);
			Debug.Log ("gamestate saved to local storage");
		}

		//set all saved games
		public void setSavedGames(List<SavedGame> games)
		{
			savedGames = games;
		}
			



        #endregion question data

        public void Save(int id, string username, string email, string password, string dob, string questionsSubmitted,
			int numQuestionsSubmitted, int numGamesPlayed, int highestScore, int numCorrectAnswers, int totalQuestionsAnswered)
        {
            SetId(id);
            SetUsername(username);
            SetEmail(email);
            SetPassword(password);
            SetDOB(dob);
            SetQuestionsSubmitted(questionsSubmitted);
            SetNumberQuestionsSubmitted(numQuestionsSubmitted);
            SetGamesPlayed(numGamesPlayed);
            //SetHighestScore(highestScore);
            //SetNumberCorrectAnswers(numCorrectAnswers);
            SetTotalQuestionsAnswered(totalQuestionsAnswered);
        }

        public void Load()
        {
            _player = new Player();

            // load player type
            if (PlayerPrefs.HasKey(_typeKey))
                _type = PlayerPrefs.GetInt(_typeKey);

            // load player username
            if (PlayerPrefs.HasKey(_usernameKey))
                _player.username = PlayerPrefs.GetString(_usernameKey);

            // load player email
            if (PlayerPrefs.HasKey(_emailKey))
                _player.email = PlayerPrefs.GetString(_emailKey);

            // load player username
            if (PlayerPrefs.HasKey(_passwordKey))
                _player.password = PlayerPrefs.GetString(_passwordKey);

            // load player question data
            if (PlayerPrefs.HasKey(_questionDataKey))
                _questionData = PlayerPrefs.GetString(_questionDataKey);
        }

        public void Load(Player p)
        {
            SetId(p.ID);
            SetUsername(p.username);
            SetEmail(p.email);
            SetPassword(p.password);
            SetDOB(p.DOB);
            SetQuestionsSubmitted(p.questionsSubmitted);
            SetNumberQuestionsSubmitted(p.numQuestionsSubmitted);
            SetGamesPlayed(p.numGamesPlayed);
            SetHighestScore(p.HighestScore);
            SetTotalQuestionsAnswered(p.totalQuestionsAnswered);
            setTotalPointsScored(p.totalPointsScore);
            setTotalCorrectAnswers(p.TotalCorrectAnswers);
        }

        private void setTotalCorrectAnswers(int tot)
        {
            _player.TotalCorrectAnswers = tot;
        }

        private void setTotalPointsScored(int points)
        {
            _player.totalPointsScore = points;
        }

        //method to take raw question data and create local pool of questions inside the playercontroller.
    }
}