namespace _LetsQuiz
{
    [System.Serializable]
    public class OngoingGamesData
    {
        public int gameNumber;
        public string player;
		public string opponent;
		public string askedQuestions;
		public string QuestionsLeftInCatagory;
		public string Round1Catagory;
		public string Round2Catagory;
		public int playerScore;
		public int opponentScore;
		public bool gameRequiresOppoent;
		public int turnNumber;

    }
}