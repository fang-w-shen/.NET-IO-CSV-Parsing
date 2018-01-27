using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
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
				// Console.WriteLine("Name: " +player.FirstName + " PPG: " + player.PointsPerGame);
				List<NewsResult> newsResults = GetNews(string.Format("{0} {1}",player.FirstName,player.SecondName));
				SentimentResponse sentimentResponse = GetSentimentResponse(newsResults);
				foreach (var sentiment in sentimentResponse.Sentiments)
				{
					foreach(var newsResult in newsResults)
					{
						if (newsResult.Headline == sentiment.Id)
						{
							double score;
							if (double.TryParse(sentiment.Score, out score))
							{

								newsResult.SentimentScore = score;
							}
							break;
						}
					}
				}
				foreach (var result in newsResults)
				{

					Console.WriteLine(string.Format("Sentiment Score: {3}, Date: {0}, Headline {1}, Summary {2} \r\n", result.DatePublished, result.Headline, result.Summary, result.SentimentScore));
				}
				Console.ReadKey();
			}
			fileName = Path.Combine(directory.FullName, "topten.json");
			// Console.WriteLine(GetGoogleHomePage("Diego Valeri"));
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
		public static List<NewsResult> GetNews(string playerName)
		{

			var results = new List<NewsResult>();
			var webClient = new WebClient();
			webClient.Headers.Add("Ocp-Apim-Subscription-Key", "ef17e67f98fb46caaf6dc0d059364255");
			byte[] searchResults = webClient.DownloadData(string.Format("https://api.cognitive.microsoft.com/bing/v7.0/news/search?q={0}&mkt=en-us", playerName));
			var serializer = new JsonSerializer();
			using (var stream = new MemoryStream(searchResults))
			using (var reader = new StreamReader(stream))
			using (var jsonReader = new JsonTextReader(reader))
			{
				// var url = "https://galaxcyclopedia.herokuapp.com/solarsystem/?api_key=1";
				// var client = new RestClient(url);
				// var request = new RestRequest(Method.GET);
				// request.AddHeader("accept", "version.2.0.json");
				// IRestResponse response = client.Execute(request);
				// Console.WriteLine(response.Content);
				// return reader.ReadToEnd();
				results = serializer.Deserialize<NewsSearch>(jsonReader).NewsResults;
			}
			return results;
		}
		public static SentimentResponse GetSentimentResponse(List<NewsResult> newsResults)
		{
			var sentimentResponse = new SentimentResponse();
			var sentimentRequest = new SentimentRequest();
			sentimentRequest.Documents = new List<Document>();
			foreach (var result in newsResults)
			{
				sentimentRequest.Documents.Add(new Document { Id = result.Headline, Text = result.Summary });
			}

			var webClient = new WebClient();
			webClient.Headers.Add("Ocp-Apim-Subscription-Key", "a26df349753c400882fa9e068d0d74dd");
			webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
			webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
			string requestJson = JsonConvert.SerializeObject(sentimentRequest);
			byte[] requestBytes = Encoding.UTF8.GetBytes(requestJson);
			byte[] response = webClient.UploadData("https://eastus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment", requestBytes);
			string sentiments = Encoding.UTF8.GetString(response);
			sentimentResponse = JsonConvert.DeserializeObject<SentimentResponse>(sentiments);
			return sentimentResponse;
		}
	}
}

