using SportExerciseBattle.HTTP;
using System.Text.Json;
using HttpMethod = SportExerciseBattle.HTTP.HttpMethod;
using SportExerciseBattle.DataLayer;
using SportExerciseBattle.Models;


namespace SportExerciseBattle.APILayer
{
    public class UsersEndpoint : IHttpEndpoint
    {
        private UserDAO userDAO = new UserDAO(); // Create an instance of UserDAO
        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.POST)
            {
                CreateUser(rq, rs); // Delegate the task to UserDAO
                return true;
            }
            else if (rq.Method == HttpMethod.GET)
            {
                GetUserData(rq, rs);
                return true;
            }
            else if (rq.Method == HttpMethod.PUT)
            {
                UpdateUser(rq, rs);
                return true;
            }
            return false;
        }


        public void CreateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content ?? "");

                userDAO.CreateUser(rq, rs, user); // Delegate the task to UserDAO

                rs.ResponseCode = 201;
                rs.ResponseMessage = "OK";
            }
            catch (Exception)
            {
                rs.ResponseCode = 400;
                rs.Content = "Failed to parse User data! ";
            }
        }

        public void GetUserData(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                // Extrahieren des Benutzernamens aus dem Pfad
                var username = rq.Path.LastOrDefault();

                // Extrahieren des Tokens aus dem Authorization-Header
                var authHeader = rq.Headers.FirstOrDefault(h => h.Key.ToLower() == "authorization");
                if (authHeader.Key == null || !authHeader.Value.StartsWith("Basic "))
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Unauthorized: Empty or Wrong Startstring";
                    return;
                }

                var token = authHeader.Value.Substring("Basic ".Length);

                // Validieren des Tokens
                if (!TokenService.ValidateToken(token, username))
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Unauthorized: Wrong Token or Username";
                    return;
                }

                // Benutzerdaten abrufen (hier als Beispiel)
                var user = userDAO.GetUserByUsername(username);
                if (user == null)
                {
                    rs.ResponseCode = 404;
                    rs.Content = "User not found";
                    return;
                }

                // Benutzerdaten serialisieren und zurückgeben
                rs.Content = JsonSerializer.Serialize(user);
                rs.Headers.Add("Content-Type", "application/json");
                rs.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                rs.ResponseCode = 500;
                rs.Content = $"Internal server error: {ex.Message}";
            }
        }

        public void UpdateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                // Extrahieren des Benutzernamens aus dem Pfad
                var username = rq.Path.LastOrDefault();

                // Extrahieren des Tokens aus dem Authorization-Header
                var authHeader = rq.Headers.FirstOrDefault(h => h.Key.ToLower() == "authorization");
                if (authHeader.Key == null || !authHeader.Value.StartsWith("Basic "))
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Unauthorized: Empty or Wrong Startstring";
                    return;
                }

                var token = authHeader.Value.Substring("Basic ".Length);

                // Validieren des Tokens
                if (!TokenService.ValidateToken(token, username))
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Unauthorized: Wrong Token";
                    return;
                }

                // Extrahieren der Benutzerdaten aus dem Request
                var user = JsonSerializer.Deserialize<User>(rq.Content ?? "");

                // Aktualisieren der Benutzerdaten (hier als Beispiel)
                userDAO.UpdateUser(rq, rs, user, username);

                rs.ResponseCode = 200;
                rs.ResponseMessage = "OK";
            }
            catch (Exception ex)
            {
                rs.ResponseCode = 500;
                rs.Content = $"Internal server error: {ex.Message}";
            }
        }
    }
}
