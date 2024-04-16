using Npgsql;
using SportExerciseBattle.HTTP;
using SportExerciseBattle.Models;

namespace SportExerciseBattle.DataLayer
{
    public class HistoryDAO
    {
        public void AddEntry(HttpRequest rq, HttpResponse rs, string username, Entry entry)
        {
            try
            {


                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand(@"INSERT INTO ""history""(username, entryname, count, duration, timestamp) VALUES (@username, @entryname, @count, @duration, @timestamp)", connection))
                    {
                        cmd.Parameters.AddWithValue("username", username); //Username as foreign key
                        cmd.Parameters.AddWithValue("entryname", entry.Name);
                        cmd.Parameters.AddWithValue("count", entry.Count);
                        cmd.Parameters.AddWithValue("duration", entry.DurationInSeconds);
                        cmd.Parameters.AddWithValue("timestamp", entry.Timestamp);
                        var affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            rs.ResponseCode = 201;
                            rs.ResponseMessage = "Entry added successfully";
                        }
                        else
                        {
                            rs.ResponseCode = 400;
                            rs.Content = "Failed to add entry";
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
