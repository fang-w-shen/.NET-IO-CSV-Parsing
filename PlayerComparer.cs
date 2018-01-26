using System;
using System.Collections.Generic;
namespace SoccerStats
{

	public class PlayerComparer : IComparer<Player>
	{
		public int Compare(Player x, Player y)
		{
            return x.PointsPerGame.CompareTo(y.PointsPerGame)*-1;
			// if (x.PointsPerGame < y.PointsPerGame)
			// {
			// 	return -1;
			// }
			// else if (x.PointsPerGame == y.PointsPerGame)
			// {
			// 	return 0;
			// }

			// else if (x.PointsPerGame > y.PointsPerGame)
			// {
			// 	return 1;
			// }
			// return 0;
		}
	}


}

