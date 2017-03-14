using System;

namespace Tennis
{
	class TennisGame1 : ITennisGame
	{
		private int m_score1 = 0;
		private int m_score2 = 0;
		private string player1Name;
		private string player2Name;

		public TennisGame1(string player1Name, string player2Name)
		{
			this.player1Name = player1Name;
			this.player2Name = player2Name;
		}

		public void WonPoint(string playerName)
		{
			if (playerName == "player1")
				m_score1 += 1;
			else
				m_score2 += 1;
		}

		public string GetScore()
		{
			if (m_score1 == m_score2)
			{
				if (m_score1 < 3)
				{
					return ConvertScoreToString(m_score1) + "-" + "All";
				}

				return "Deuce";
			}
			else if (m_score1 >= 4 || m_score2 >= 4)
			{
				var minusResult = m_score1 - m_score2;
				if (minusResult == 1)
				{
					return "Advantage player1";
				}
				else if (minusResult == -1)
				{
					return "Advantage player2";
				}
				else if (minusResult >= 2)
				{
					return "Win for player1";
				}
				else
				{
					return "Win for player2";
				}
			}
			else
			{
				return ConvertScoreToString(m_score1) + "-" + ConvertScoreToString(m_score2);
			}
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

