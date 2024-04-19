using SportExerciseBattle.DataLayer;
using SportExerciseBattle.HTTP;
using SportExerciseBattle.Models;
using System.Text.Json;
using HttpMethod = SportExerciseBattle.HTTP.HttpMethod;


namespace SportExerciseBattle.APILayer
{
  
        public class HistoryEndpoint : IHttpEndpoint
        {
            private HistoryDAO historyDAO = new HistoryDAO(); // Create an instance of StatsDAO
            private TournamentManager tournamentManager = new TournamentManager();

            public bool HandleRequest(HttpRequest rq, HttpResponse rs)
            {
                if (rq.Method == HttpMethod.POST)
                {
                    AddEntry(rq, rs, historyDAO); // Delegate the task to AddEntry method
                    return true;
                }
                else if(rq.Method == HttpMethod.GET)
            {
                    GetEntries(rq, rs, historyDAO); // Delegate the task to GetStats method
                    return true;
                }
                return false;
            }

        public void AddEntry(HttpRequest rq, HttpResponse rs, HistoryDAO historyDAO)
        {
            try
            {
                var entry = JsonSerializer.Deserialize<Entry>(rq.Content ?? "");

                try
                {
                    // Extrahieren des Tokens aus dem Authorization-Header
                    var authHeader = rq.Headers.FirstOrDefault(h => h.Key.ToLower() == "authorization");
                    if (authHeader.Key == null || !authHeader.Value.StartsWith("Basic "))
                    {
                        rs.ResponseCode = 401;
                        rs.Content = "Unauthorized";
                        return;
                    }

                    var token = authHeader.Value.Substring("Basic ".Length);
                    var username = token.Split("-")[0]; // Extrahieren des Benutzernamens aus dem Token

                    // Validieren des Tokens
                    if (!TokenService.ValidateToken(token, username))
                    {
                        rs.ResponseCode = 401;
                        rs.Content = "Unauthorized";
                        return;
                    }
                    historyDAO.AddEntry(rq, rs, username, entry); // Delegate the task to UserDAO
                }
                catch (Exception ex)
                {
                    rs.ResponseCode = 500;
                    rs.Content = $"Internal server error: {ex.Message}";
                }
                if(Tournament.Instance.IsRunning == false)
                {
                    var tournament = Tournament.Instance;
                    tournamentManager.StartTournament();  //if no tournament is currently running call StartTournament method
                    tournament.Log.Add(tournament.StartTime + "Arena is open! Join the fight quickly!"); //add the start to the tournament log
                } 
                else
                {
                    var tournament = Tournament.Instance;
                    tournament.Log.Add(DateTime.Now + "Challenge accepted! A Competitor(Entry) joined the tournament"); //add the entry to the tournament log
                }
                rs.ResponseCode = 201;
                rs.ResponseMessage = "OK";
            }
            catch (Exception)
            {
                rs.ResponseCode = 400;
                rs.Content = "Failed to parse User data! ";
            }
        }

        public void GetEntries(HttpRequest rq, HttpResponse rs, HistoryDAO historyDAO)
        {
            try
            {
                // Extrahieren des Tokens aus dem Authorization-Header
                var authHeader = rq.Headers.FirstOrDefault(h => h.Key.ToLower() == "authorization");
                if (authHeader.Key == null || !authHeader.Value.StartsWith("Basic "))
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Unauthorized";
                    return;
                }

                var token = authHeader.Value.Substring("Basic ".Length);
                var username = token.Split("-")[0]; // Extrahieren des Benutzernamens aus dem Token

                // Validieren des Tokens
                if (!TokenService.ValidateToken(token, username))
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Unauthorized";
                    return;
                }

                // Einträge abrufen
                var entriesList = historyDAO.GetEntries(username);
                if (entriesList == null || entriesList.Count == 0)
                {
                    rs.ResponseCode = 404;
                    rs.Content = "currently empty";
                    return;
                }

                // Statistiken serialisieren und zurückgeben
                rs.Content = JsonSerializer.Serialize(entriesList);
                rs.Headers.Add("Content-Type", "application/json");
                rs.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                rs.ResponseCode = 500;
                rs.Content = $"Internal server error: {ex.Message}";
            }
        }
    }
}

