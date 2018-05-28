namespace _LetsQuiz
{
    [System.Serializable]
    public class Player
    {
        public int ID = 0;
        public string username = "username";
        public string email = "email";
        public string password = "password";
        public string DOB = "dob";
        public string questionsSubmitted = "questions";
        public int numQuestionsSubmitted = 0;
        public int numGamesPlayed = 0;
        public int highestScore = 0;
        public int numCorrectAnswers = 0;
        public int totalQuestionsAnswered = 0;
    }
}

