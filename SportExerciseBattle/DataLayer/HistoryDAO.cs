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
                        cmd.Parameters.AddWithValue("entryname", entry.EntryName);
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
                StatsDAO statsDAO = new StatsDAO();
                statsDAO.AddToCount(username, entry.Count);
            }
            catch (Exception ex)
            {
                rs.ResponseCode = 500;
                rs.Content = $"Internal server error: {ex.Message}";
            }
        }

        public List<Entry> GetEntries(string username)
        {
            List<Entry> entryList = new List<Entry>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("SELECT username, entryname, count, duration, timestamp FROM history WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var entry = new Entry
                                {
                                    Username = reader.GetString(0),
                                    EntryName = reader.GetString(1),
                                    Count = reader.GetInt32(2),
                                    DurationInSeconds = reader.GetInt32(3),
                                    Timestamp = reader.GetDateTime(4)
                                };
                                entryList.Add(entry);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                Console.WriteLine($"Fehler beim Abrufen der Statistiken: {ex.Message}");
            }

            return entryList;
        }
    }
}
