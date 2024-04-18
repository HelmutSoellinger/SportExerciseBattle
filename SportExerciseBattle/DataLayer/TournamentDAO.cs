
using Npgsql;
using SportExerciseBattle.Models;
namespace SportExerciseBattle.DataLayer
{
    public class TournamentDAO
    {
        public void GetParticipants()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("SELECT username FROM history WHERE timestamp " +
                        "BETWEEN @start_timestamp AND @end_timestamp GROUP BY username;", connection))
                    {
                        cmd.Parameters.AddWithValue("start_timestamp", tournament.StartTime);
                        cmd.Parameters.AddWithValue("end_timestamp", tournament.StartTime.AddMinutes(2));

                        using (var reader = cmd.ExecuteReader())
                        {
                            var uniqueParticipants = new HashSet<string>(); //ein HashSet, um Duplikate zu vermeiden
                            while (reader.Read())
                            {
                                uniqueParticipants.Add(reader.GetString(0));
                            }
                            tournament.Participants = uniqueParticipants.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                Console.WriteLine($"Fehler beim Abrufen der Teilnehmer: {ex.Message}");
            }
        }
        public void GetLeaders()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    // Determine the highest pushup count.
                    using (var cmd = new NpgsqlCommand("SELECT MAX(total_pushups) AS max_pushups FROM (" +
                        "SELECT username, SUM(count) AS total_pushups FROM history " +
                        "WHERE timestamp BETWEEN @start_timestamp AND @end_timestamp GROUP BY username)" +
                        " AS subquery;", connection))
                    {
                        cmd.Parameters.AddWithValue("start_timestamp", tournament.StartTime);
                        cmd.Parameters.AddWithValue("end_timestamp", tournament.StartTime.AddMinutes(2));

                        int maxPushups = 0;
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                maxPushups = reader.GetInt32(0);
                            }
                        }

                        // Identify all participants with the highest pushup count.
                        using (var drawCmd = new NpgsqlCommand("SELECT username FROM (" +
                            "SELECT username, SUM(count) AS total_pushups FROM history " +
                            "WHERE timestamp BETWEEN @start_timestamp AND @end_timestamp GROUP BY username)" +
                            " AS subquery WHERE total_pushups = @max_pushups;", connection))
                        {
                            drawCmd.Parameters.AddWithValue("start_timestamp", tournament.StartTime);
                            drawCmd.Parameters.AddWithValue("end_timestamp", tournament.StartTime.AddMinutes(2));
                            drawCmd.Parameters.AddWithValue("max_pushups", maxPushups);

                            var uniqueLeadingUsers = new HashSet<string>();
                            using (var reader = drawCmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    uniqueLeadingUsers.Add(reader.GetString(0));
                                }
                                tournament.LeadingUsers = uniqueLeadingUsers.ToList();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                Console.WriteLine($"Fehler beim Abrufen des Gewinners/Gewinner: {ex.Message}");
            }
        }
        
        public void UpdateElo()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                    // If there is a draw, all leading users get +1 Elo.
                    // Otherwise, the leading user gets +2 Elo, and all others get -1 Elo.
                    if (tournament.LeadingUsers.Count > 1)
                    {
                        // Handle draw scenario
                        using (var cmd = new NpgsqlCommand("UPDATE person SET elo = elo + 1 WHERE username = ANY(@leadingUsernames);" +
                            "UPDATE person SET elo= elo-1 WHERE username = ANY(@usernames) and username != ANY (@winnerUsername)", connection))
                        {
                            cmd.Parameters.AddWithValue("@usernames", NpgsqlTypes.NpgsqlDbType.Array
                                | NpgsqlTypes.NpgsqlDbType.Text, tournament.Participants.ToArray()); // liste in textarray für SQL umwandeln
                            cmd.Parameters.AddWithValue("@winnerUsername", NpgsqlTypes.NpgsqlDbType.Array
                                | NpgsqlTypes.NpgsqlDbType.Text, tournament.LeadingUsers.ToArray());
                            var affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                Console.WriteLine("Elo updated successfully for draw scenario.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to update Elo for draw scenario.");
                            }
                        }
                    }
                    else
                    {
                        {
                            using (var cmd = new NpgsqlCommand("UPDATE person SET elo = elo + 2 WHERE username = ANY(@winnerUsername);" +
                                "UPDATE person SET elo= elo-1 WHERE username = ANY(@usernames) and username !=ANY(@winnerUsername)", connection))
                            {
                                string participants = string.Join(",", tournament.Participants);
                                cmd.Parameters.AddWithValue("@usernames", NpgsqlTypes.NpgsqlDbType.Array 
                                    | NpgsqlTypes.NpgsqlDbType.Text, tournament.Participants.ToArray());
                                cmd.Parameters.AddWithValue("@winnerUsername", NpgsqlTypes.NpgsqlDbType.Array 
                                    | NpgsqlTypes.NpgsqlDbType.Text, tournament.LeadingUsers.ToArray());


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
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                Console.WriteLine($"Fehler beim Aktualisieren des Elos: {ex.Message}");
            }
        }
        
        public void UpdateTournamentStats()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    if (tournament.LeadingUsers.Count > 1)
                    {
                        // Handle draw scenario
                        using (var updateCmd = new NpgsqlCommand("UPDATE person SET draws = draws + 1 WHERE username = ANY(@usernames);" + //besssere Lösung wie Oben :)
                            "UPDATE person SET losses= losses+1 WHERE username != ALL(@usernames)", connection))
                        {
                            updateCmd.Parameters.AddWithValue("@usernames", NpgsqlTypes.NpgsqlDbType.Array 
                                | NpgsqlTypes.NpgsqlDbType.Text, tournament.LeadingUsers.ToArray()); 
                            var affectedRows = updateCmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                Console.WriteLine("TournamentStats updated successfully for draw scenario.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to update TurnierStats for draw scenario.");
                            }
                        }
                    }
                    else
                    {
                        using (var cmd = new NpgsqlCommand("UPDATE person SET wins = wins + 1 WHERE username = ANY(@usernames);" +
                            "UPDATE person SET losses= losses+1 WHERE username != ALL(@usernames)", connection))
                        {
                            cmd.Parameters.AddWithValue("@usernames", NpgsqlTypes.NpgsqlDbType.Array 
                                | NpgsqlTypes.NpgsqlDbType.Text, tournament.LeadingUsers.ToArray()); 
                            var affectedRows = cmd.ExecuteNonQuery();

                            if (affectedRows > 0)
                            {
                                Console.WriteLine("Tournament stats updated successfully");
                            }
                            else
                            {
                                Console.WriteLine("Failed to update tournament stats");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                Console.WriteLine($"Fehler beim Aktualisieren der Turnierstatistiken: {ex.Message}");
            }
        }
    }
}

