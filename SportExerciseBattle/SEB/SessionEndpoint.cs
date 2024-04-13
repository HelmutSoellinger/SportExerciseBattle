using SportExerciseBattle.HTTP;
using System.Text.Json;
using HttpMethod = SportExerciseBattle.HTTP.HttpMethod;
using SportExerciseBattle.DataLayer;
using Npgsql;
using SportExerciseBattle.Data_Layer;

namespace SportExerciseBattle.SEB
{
    public class SessionsEndpoint : IHttpEndpoint
    {
        private SessionDAO SessionDAO = new SessionDAO(); // Create an instance of UserDAO

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.POST)
            {
                Login(rq, rs); // Delegate the task to Login method
                return true;
            }
            return false;
        }

       
    public void Login(HttpRequest rq, HttpResponse rs)
    {
       try
        {
            var loginRequest = JsonSerializer.Deserialize<User>(rq.Content ?? "");
                if(SessionDAO.Login(rq, rs, loginRequest))                    // Delegate the task to SessionDOA
                {
                    using (var connection = DatabaseConnection.GetConnection())
                    {
                        var cmdtoken = new NpgsqlCommand(@"UPDATE ""person"" SET token = @token WHERE username = @username", connection);
                        cmdtoken.Parameters.AddWithValue("token", "Basic " + loginRequest.Username + "-sebToken");
                        cmdtoken.Parameters.AddWithValue("username", loginRequest.Username);
                        cmdtoken.ExecuteNonQuery();
                    }
                rs.ResponseCode = 200;
                rs.ResponseMessage = "OK";
            }
                
        }
        catch (Exception)
        {
            rs.ResponseCode = 400;
            rs.Content = "Failed to parse login data!";
        }
                
    }
    }
}
