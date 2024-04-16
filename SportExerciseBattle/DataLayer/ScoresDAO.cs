using Npgsql;
using SportExerciseBattle.Data_Layer;
using SportExerciseBattle.Models;

namespace SportExerciseBattle.DataLayer
{
    public class ScoresDAO
    {
        public List<Stats> GetScores()
        {
            List<Stats> statsList = new List<Stats>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("SELECT username, name, elo, count FROM person ORDER BY elo DESC", connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var stats = new Stats
                                {
                                    Username = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Elo = reader.GetInt32(2),
                                    Count = reader.GetInt32(3)
                                };
                                statsList.Add(stats);
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

            return statsList;
        }
    }
}
