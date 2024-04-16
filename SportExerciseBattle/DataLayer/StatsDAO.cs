using Npgsql;
using SportExerciseBattle.Models;

namespace SportExerciseBattle.DataLayer
{
    public class StatsDAO
    {
        public Stats GetStats(string username)
        {
            Stats stats = null;

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("SELECT username, name, elo, count FROM person WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                stats = new Stats
                                {
                                    Username = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Elo = reader.GetInt32(2),
                                    Count = reader.GetInt32(3)
                                };
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

            return stats;
        }
    }
}
