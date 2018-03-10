using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class RoundData
    {
        public string Name;
        public int TimeLimitSeconds;
        public int CorrectAnswerPoints;
        public QuestionData[] Questions;
    }
}