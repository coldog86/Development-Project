	[System.Serializable]
	public class HighScoreData
	{
		public HighScore[] highscoreData;
	}

	[System.Serializable]
	public class HighScore
	{
		public int userID;
		public string userName;
		public int gamesWon;
		public int questionRight;
		public int totalScore;

	}


