using Npgsql;
using SportExerciseBattle.Data_Layer;
using SportExerciseBattle.HTTP;
using SportExerciseBattle.Models;

namespace SportExerciseBattle.DataLayer
{
    internal class SessionDAO
    {
            public bool Login(HttpRequest rq, HttpResponse rs, User loginRequest)
            {
                try
                {
                   // var loginRequest = JsonSerializer.Deserialize<User>(rq.Content ?? "");

                    using (var connection = DatabaseConnection.GetConnection())
                    {
                        using (var cmd = new NpgsqlCommand("SELECT password FROM person WHERE username = @username", connection))
                        {
                            cmd.Parameters.AddWithValue("username", loginRequest.Username);
                        using (var reader = cmd.ExecuteReader())
                        { 

                            if (reader.Read())
                            {

                                string storedPassword = reader.GetString(0);


                                string Password = loginRequest.Password;
                                if (storedPassword == Password)
                                {
                                    
                                    rs.ResponseCode = 200;
                                    rs.ResponseMessage = "Login successful";
                                    return true;
                                }
                                else
                                {
                                    rs.ResponseCode = 401;
                                    rs.Content = "Invalid username or password";
                                    return false;
                                }
                            }
                            else
                            {
                                rs.ResponseCode = 401;
                                rs.Content = "Invalid username or password";
                                return false;
                            }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    rs.ResponseCode = 500;
                    rs.Content = $"Internal server error: {ex.Message}";
                    return false;
                }
            }

        /*

            // Example method to hash a password. Implement your own hashing logic.
            private string HashPassword(string password)
            {
                // Implement your password hashing logic here
                // For example, using SHA256
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                    var hash = sha256.ComputeHash(bytes);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }
    }
        */
}
}

