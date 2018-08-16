using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace _LetsQuiz
{
    public class QuestionController : MonoBehaviour
    {
        #region variables

        [Header("Components")]
        private PlayerController _playerController;
		private DataController _dataController;
		private GameLobbyController _gameLobbyController;

        private string _questionData;
        private GameData _allQuestions;
        private List<QuestionData> _AskedQuestions;

        #endregion variables

        #region methods

        public void Load()
        {
            Debug.Log("[QuestionController] Load()");
            _playerController = FindObjectOfType<PlayerController>();
            _questionData = _playerController.GetQuestionData();
        }

        public GameData extractQuestionsFromJSON()
        {
            GameData allQ = JsonUtility.FromJson<GameData>(_questionData);
            return allQ;
        }

        public RoundData extractQuestionsFromJSON(string json) //TODO Pick up from here. not sure what we need to deserialize this into, probably Round data but there is no name so maybe a new object....
        {
            RoundData allQ = JsonUtility.FromJson<RoundData>(json);
            return allQ;
        }

        public QuestionData[] extractQuestionsFromJSON(int catagory)
        {
            GameData allQ = JsonUtility.FromJson<GameData>(_questionData);
            QuestionData[] allQuestionsInCatagory = allQ.allRoundData[catagory].questions;
            return allQuestionsInCatagory;
        }

        public GameData getAllQuestions()
        {
            return _allQuestions;
        }

        public QuestionData[] getAllQuestionsAllCatagories()
        {
            List<QuestionData> questionsList = new List<QuestionData>();
            GameData allQ = JsonUtility.FromJson<GameData>(_questionData);
            for (int i = 0; i < allQ.allRoundData.Length; i++)
            {
                for (int n = 0; n < allQ.allRoundData[i].questions.Length; n++)
                {
                    questionsList.Add(allQ.allRoundData[i].questions[n]);
                }
            }
            QuestionData[] allQuestionsAllCatagories = questionsList.ToArray();

            return allQuestionsAllCatagories;
        }

		public List<string> getAllCatagories()
		{
			List<string> catagoryList = new List<string>();
			GameData allQ = JsonUtility.FromJson<GameData>(_questionData);
			for (int i = 0; i < allQ.allRoundData.Length; i++)
			{
				catagoryList.Add(allQ.allRoundData[i].name);
			}
			return catagoryList;
		}


		public List<string> removeCatagory(List<string> catagories, string remove)
		{
			for (int i = 0; i < catagories.Count; i++) {
				if(catagories[i].Equals(remove))
					catagories.RemoveAt(i);
			}
			return catagories;
		}

		public string getRandomCatagory()
		{
			_dataController = FindObjectOfType<DataController> ();
			_gameLobbyController = FindObjectOfType<GameLobbyController> ();
			List<string> catagoryList = getAllCatagories ();
			catagoryList = removeCatagory(catagoryList, _dataController.ongoingGameData.Round1Catagory);
			catagoryList = removeCatagory(catagoryList, _dataController.ongoingGameData.Round2Catagory);
			int randomNumber = Random.Range(0, catagoryList.Count - 1); //gets random number between 0 and total number of questions
			string catagory = catagoryList[randomNumber];
			_dataController.catagory = catagory;
			return catagory;
		}


		public QuestionData[] getQuestionsInCatagory(string catagory)
		{
			QuestionData[] questionsInCatagory = null;
			GameData allQ = JsonUtility.FromJson<GameData>(_questionData);
			for (int i = 0; i < allQ.allRoundData.Length; i++){			
				if (allQ.allRoundData [i].name.Equals (catagory)) {
					questionsInCatagory = allQ.allRoundData [i].questions;
					return questionsInCatagory;
				}
			}
			return questionsInCatagory;
		}

		public QuestionData[] getQuestionsInCatagory(int selection)
		{
			QuestionData[] questionsFromCatagory;
			GameData allQ = JsonUtility.FromJson<GameData>(_questionData);
			questionsFromCatagory = allQ.allRoundData [selection].questions;
			return questionsFromCatagory;
		}

        public QuestionData[] removeQuestion(QuestionData[] questionPool, int remove)
        {
            List<QuestionData> poolAsList = new List<QuestionData>();

            for (int i = 0; i < questionPool.Length; i++)
                poolAsList.Add(questionPool[i]);

            poolAsList.RemoveAt(remove);
            questionPool = poolAsList.ToArray();
            return questionPool;
        }

        public void addAskedQuestionToAskedQuestions(QuestionData currentQuestion)
        {
            try
            {
                _AskedQuestions.Add(currentQuestion);
            }
            catch (Exception e)
            {
                Debug.Log("[QuestionController] addAskedQuestionToAskedQuestions() Error : " + e.StackTrace);

                _AskedQuestions = new List<QuestionData>
                {
                    currentQuestion
                };
            }
        }

        public string getAskedQuestions()
        {
            RoundData _RoundQuestions = new RoundData();
            _RoundQuestions.name = "question list for opponent";
            _RoundQuestions.questions = _AskedQuestions.ToArray();
            string askedQuestionsAsJSON = JsonUtility.ToJson(_RoundQuestions);
            Debug.Log("askedQuestionsJSON = " + askedQuestionsAsJSON);
            _AskedQuestions = new List<QuestionData>(); //clear the list
            return askedQuestionsAsJSON;
        }

        public string getRemainingQuestions(QuestionData[] questionPool)
        {
            RoundData _RemainingQuestions = new RoundData();
            _RemainingQuestions.name = "remaining questions in catagory, incase opponent answers all the 'askedQuestions' ";
            _RemainingQuestions.questions = questionPool;
            string remainingQuestionsAsJSON = JsonUtility.ToJson(_RemainingQuestions);
            Debug.Log("_RemainingQuestions = " + remainingQuestionsAsJSON);
            return remainingQuestionsAsJSON;
        }

        #endregion methods
    }
}