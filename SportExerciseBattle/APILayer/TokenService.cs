using Npgsql;
using SportExerciseBattle.DataLayer;
using SportExerciseBattle.Models;

namespace SportExerciseBattle.APILayer
{
    public static class TokenService
    {

        public static void GenerateToken(User? loginRequest)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                var cmdtoken = new NpgsqlCommand(@"UPDATE ""person"" SET token = @token WHERE username = @username", connection);
                cmdtoken.Parameters.AddWithValue("token", loginRequest.Username + "-sebToken");
                cmdtoken.Parameters.AddWithValue("username", loginRequest.Username);
                cmdtoken.ExecuteNonQuery();
            }
        }
        public static bool ValidateToken(string token, string username)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                using (var cmd = new NpgsqlCommand("SELECT token FROM person WHERE username = @username", connection))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if ((reader.Read()))
                        {


                            // Extrahieren des Tokens aus der Datenbank
                            var dbToken = reader.GetString(0);

                            // Vergleichen des gesendeten Tokens mit dem Token aus der Datenbank
                            return token == dbToken;
                        }
                        else
                        {
                            // Benutzer nicht gefunden, also ungültiges Token
                            return false;
                        }
                    }
                }
            }
        }
    }
}