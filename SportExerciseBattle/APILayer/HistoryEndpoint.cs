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

            public bool HandleRequest(HttpRequest rq, HttpResponse rs)
            {
                if (rq.Method == HttpMethod.POST)
                {
                    AddEntry(rq, rs, historyDAO); // Delegate the task to AddEntry method
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

                    //if no tournament is running in database call StartTournament method, 
                    //else call add entry to tournament 
                    //(maybe nicht notwendig, weil tournament automatisch endet und 
                    //count und winner in end tournament logik behandelt werden)
                }
                catch (Exception ex)
                {
                    rs.ResponseCode = 500;
                    rs.Content = $"Internal server error: {ex.Message}";
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
    }
}
