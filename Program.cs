using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
            fileName = Path.Combine(directory.FullName, "players.json");
            var players = DeserializePlayers(fileName);
            var topTenPlayers = GetTopTenPlayers(players);
            foreach (var player in topTenPlayers)
            {
                Console.WriteLine("Name: " +player.FirstName + " PPG: " + player.PointsPerGame);
            }
            fileName = Path.Combine(directory.FullName, "topten.json");
            SerializePlayerToFile(topTenPlayers, fileName);
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

				}
			}
			return soccerResults;
		}
		public static List<Player> DeserializePlayers(string fileName)
		{
			var players = new List<Player>();
			var serializer = new JsonSerializer();
			using (var reader = new StreamReader(fileName))
			using (var jsonReader = new JsonTextReader(reader))
			{

                players = serializer.Deserialize<List<Player>>(jsonReader);

			}
			return players;
		}
        public static List<Player> GetTopTenPlayers(List<Player> players)
        {
            var topTenPlayers = new List<Player>();
            players.Sort(new PlayerComparer());
            for(var i=0;i<10;i++)
            {
                topTenPlayers.Add(players[i]);
            }
            return topTenPlayers;
        }
        public static void SerializePlayerToFile(List<Player> players, string fileName)
        {
  
            var serializer = new JsonSerializer();
            using (var writer = new StreamWriter(fileName))
            using (var jsonWriter = new JsonTextWriter(writer))
            {

                serializer.Serialize(jsonWriter, players);

            }
 
        }
	}
}

