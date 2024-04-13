using Npgsql;
using SportExerciseBattle.Data_Layer;
using SportExerciseBattle.HTTP;
using SportExerciseBattle.SEB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
                    using (var cmd = new NpgsqlCommand(@"INSERT INTO ""person""(username, password) VALUES (@username, @password)", connection))
                    {
                        cmd.Parameters.AddWithValue("username", user.Username);
                        cmd.Parameters.AddWithValue("password", user.Password); // Consider hashing the password before storing
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

    }
}
