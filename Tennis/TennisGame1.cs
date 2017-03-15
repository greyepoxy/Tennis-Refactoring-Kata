using System;

namespace Tennis
{
	class TennisGame1 : ITennisGame
	{
		private readonly TennisGameState _tennisGameState = new TennisGameState();

		public void WonPoint(string playerName)
		{
			if (playerName == "player1")
				_tennisGameState.Player1Score += 1;
			else
				_tennisGameState.Player2Score += 1;
		}

		public string GetScore()
		{
			if (_tennisGameState.Player1Score == _tennisGameState.Player2Score)
			{
				if (_tennisGameState.Player1Score < 3)
				{
					return ConvertScoreToString(_tennisGameState.Player1Score) + "-" + "All";
				}

				return "Deuce";
			}
			if (_tennisGameState.Player1Score >= 4 || _tennisGameState.Player2Score >= 4)
			{
				var minusResult = _tennisGameState.Player1Score - _tennisGameState.Player2Score;
				if (minusResult == 1)
				{
					return "Advantage player1";
				}
				if (minusResult == -1)
				{
					return "Advantage player2";
				}
				if (minusResult >= 2)
				{
					return "Win for player1";
				}
				return "Win for player2";
			}
			return ConvertScoreToString(_tennisGameState.Player1Score) + "-" + ConvertScoreToString(_tennisGameState.Player2Score);
		}

		private static string ConvertScoreToString(int score)
		{
			switch (score)
			{
				case 0:
					return "Love";
				case 1:
					return "Fifteen";
				case 2:
					return "Thirty";
				case 3:
					return "Forty";
			}
			throw new ArgumentOutOfRangeException(nameof(score));
		}
	}
}

