using SportExerciseBattle.HTTP;
using System.Text.Json;
using HttpMethod = SportExerciseBattle.HTTP.HttpMethod;
using SportExerciseBattle.DataLayer;

namespace SportExerciseBattle.APILayer
{
    public class StatsEndpoint : IHttpEndpoint
    {
        private StatsDAO statsDAO = new StatsDAO(); // Create an instance of StatsDAO

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.GET)
            {
                GetStats(rq, rs, statsDAO); // Delegate the task to GetStats method
                return true;
            }
            return false;
        }


        public void GetStats(HttpRequest rq, HttpResponse rs, StatsDAO statsDAO)
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
                if (!TokenService.ValidateToken(token,username))
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Unauthorized";
                    return;
                }

                // Statistiken abrufen
                var stats = statsDAO.GetStats(username);
                if (stats == null)
                {
                    rs.ResponseCode = 404;
                    rs.Content = "Stats not found";
                    return;
                }

                // Statistiken serialisieren und zurückgeben
                rs.Content = JsonSerializer.Serialize(stats);
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
