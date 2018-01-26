using System;
using System.Collections.Generic;
using System.IO;
namespace SoccerStats
{
	class Program
	{
		static void Main(string[] args)
		{

			string currentDirectory = Directory.GetCurrentDirectory();

			DirectoryInfo directory = new DirectoryInfo(currentDirectory);
			var fileName = Path.Combine(directory.FullName, "SoccerGameResults.csv");
			// Console.WriteLine(fileName);

			// var file = new FileInfo(fileName);
			// Console.WriteLine(file);
			var fileContents =  ReadSoccerResults(fileName);
			// Console.WriteLine(fileContents);
			// string[] fil	eLines = fileContents.Split(new char[]{'\r','\n'}, StringSplitOptions.RemoveEmptyEntries);
			// foreach(var line in fileLines)
			// {
			// 	Console.WriteLine(line);

			// }
			// if(file.Exists)
			// {

			// 	using(var reader = new StreamReader(fileName))
			// 	{
			// 		Console.SetIn(reader);
			// 		Console.WriteLine(Console.ReadLine());
			// 	}
			// }




			// var mysteryMessage = new byte[] { 89, 0, 97, 0, 121, 0, 33, 0 };
			// var messageContents = UnicodeEncoding.Unicode.GetString(mysteryMessage);
			// Console.WriteLine(messageContents);

		}
		public static string ReadFile(string fileName)
		{
			using(var reader = new StreamReader(fileName))
			{
				return reader.ReadToEnd();
			}
		}
		public static List<GameResult> ReadSoccerResults(string fileName)
		{
			var soccerResults = new List<GameResult>();
			using(var reader = new StreamReader(fileName))
			{
				string line = "";
				reader.ReadLine();
				while ((line = reader.ReadLine()) != null)
				{
					var gameResult = new GameResult();
					string[] values = line.Split(',');
					DateTime gameDate;
					if (DateTime.TryParse(values[0], out gameDate))
					{
						gameResult.GameDate = gameDate;
					}
					gameResult.TeamName = values[1];
					HomeOrAway homeOrAway;
					if (Enum.TryParse(values[2], out homeOrAway))
					{
						gameResult.HomeOrAway = homeOrAway;
					}
					int parseInt;
					if (int.TryParse(values[3],out parseInt))
					{
						gameResult.Goals = parseInt;
					}
					if (int.TryParse(values[4],out parseInt))
					{
						gameResult.GoalAttempts = parseInt;
					}
					if (int.TryParse(values[5],out parseInt))
					{
						gameResult.ShotsOnGoal = parseInt;
					}
					if (int.TryParse(values[6],out parseInt))
					{
						gameResult.ShotsOffGoal = parseInt;
					}
					double possessionPercent;
					if (double.TryParse(values[7], out possessionPercent))
					{
						gameResult.PossessionPercent = possessionPercent;
					}
					soccerResults.Add(gameResult);
					Console.WriteLine(soccerResults.GetType());
				}
			}
			return soccerResults;
		}
	}
}

