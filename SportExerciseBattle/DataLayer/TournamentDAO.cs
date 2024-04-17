
using Npgsql;
using SportExerciseBattle.Models;
namespace SportExerciseBattle.DataLayer
{
    internal class TournamentDAO
    {
        public void GetLeader()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("SELECT username FROM(" +
                        "SELECT username, SUM(count) AS total_pushups FROM history " +
                        "WHERE timestamp BETWEEN @start_timestamp AND @end_timestamp GROUP BY username)" +
                        " AS subquery ORDER BY total_pushups DESC LIMIT 1;", connection))
                    {
                        cmd.Parameters.AddWithValue("start_timestamp", tournament.StartTime);
                        cmd.Parameters.AddWithValue("end_timestamp", tournament.StartTime.AddMinutes(2));

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tournament.LeadingUser = reader.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                Console.WriteLine($"Fehler beim Abrufen des Gewinners: {ex.Message}");
            }
        }
        
        public void UpdateElo()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("UPDATE person SET elo = elo + 1 WHERE username = @winnerUsername;UPDATE person SET elo= elo-1 WHERE username IN(@usernames) and username !=@winnerUsername", connection))
                    {
                        string participants = string.Join(",", tournament.Participants);
                        cmd.Parameters.AddWithValue("@usernames", participants);
                        cmd.Parameters.AddWithValue("@winnerUsername", tournament.LeadingUser);

                        var affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            Console.WriteLine("Elo updated successfully");
                        }
                        else
                        {
                            Console.WriteLine("Failed to update Elo");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                Console.WriteLine($"Fehler beim Aktualisieren des Elos: {ex.Message}");
            }
        }   
/*
        public void GetAllEntriesInTournament()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("SELECT username, SUM(count) AS total_pushups FROM history WHERE timestamp BETWEEN 'start_timestamp' AND 'end_timestamp' GROUP BY username ORDER BY total_pushups DESC;", connection))
                    {
                        cmd.Parameters.AddWithValue('start_timestamp', tournament.StartTime);
                        cmd.Parameters.AddWithValue('end_timestamp', tournament.StartTime.AddMinutes(2));

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                Console.WriteLine($"Fehler beim Abrufen der Entries: {ex.Message}");
            }
        }
*/
    }
}

