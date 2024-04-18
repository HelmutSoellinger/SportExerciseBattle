using Npgsql;
using SportExerciseBattle.Models;

namespace SportExerciseBattle.DataLayer
{
    public class TournamentStatsDAO
    {
        public TournamentStats GetTournamentStats(string username)
        {
            TournamentStats tournamentStats = null;

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("SELECT wins, draws, losses FROM person WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tournamentStats = new TournamentStats
                                {
                                    Wins = reader.GetInt32(0),
                                    Draws = reader.GetInt32(1),
                                    Losses = reader.GetInt32(2),
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

            return tournamentStats;
        }

        public void AddToCount(string username, int count)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("UPDATE person SET count = count + @count WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("count", count);
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                Console.WriteLine($"Fehler beim Hinzufügen des Zählers: {ex.Message}");
            }
        }
    }
}
