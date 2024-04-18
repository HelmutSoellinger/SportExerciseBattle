using Npgsql;
using SportExerciseBattle.HTTP;
using SportExerciseBattle.Models;

namespace SportExerciseBattle.DataLayer
{
    internal class UserDAO
    {

        public void CreateUser(HttpRequest rq, HttpResponse rs, User user)
        {
            try
            {
                

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand(@"INSERT INTO ""person""(username, password, name) VALUES (@username, @password, @name)", connection))
                    {
                        cmd.Parameters.AddWithValue("username", user.Username);
                        cmd.Parameters.AddWithValue("password", user.Password); // Consider hashing the password before storing
                        cmd.Parameters.AddWithValue("name", user.Username);
                        var affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            rs.ResponseCode = 201;
                            rs.ResponseMessage = "User created successfully";
                        }
                        else
                        {
                            rs.ResponseCode = 400;
                            rs.Content = "Failed to create user";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rs.ResponseCode = 500;
                rs.Content = $"Internal server error: {ex.Message}";
            }
        }

        public User GetUserByUsername(string username)
        {
            User user = null;

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand(@"SELECT username, bio, image, name FROM ""person"" WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Username = reader.GetString(0),
                                    Password = "****", // Beachten Sie, dass das Passwort hier aus Sicherheitsgründen nicht zurückgegeben werden sollte.
                                    Bio = reader.IsDBNull(1) ? null : reader.GetString(1), // Überprüfen, ob 'bio' null ist, und setzen Sie es entsprechend.
                                    Image = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    Name = reader.IsDBNull(3) ? null : reader.GetString(3)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung, z.B. Logging oder Fehlermeldung an den Client senden
                Console.WriteLine($"Fehler beim Abrufen des Benutzers: {ex.Message}");
            }

            return user;
        }

        public void UpdateUser(HttpRequest rq, HttpResponse rs, User user, string username)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand(@"UPDATE ""person"" SET bio = @bio, image = @image, name = @name WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("bio", user.Bio);
                        cmd.Parameters.AddWithValue("image", user.Image);
                        cmd.Parameters.AddWithValue("name", user.Name);
                        cmd.Parameters.AddWithValue("username", username);
                        var affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            rs.ResponseCode = 200;
                            rs.ResponseMessage = "User updated successfully";
                        }
                        else
                        {
                            rs.ResponseCode = 400;
                            rs.Content = "Failed to update user";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rs.ResponseCode = 500;
                rs.Content = $"Internal server error: {ex.Message}";
            }
        }

    }
}
