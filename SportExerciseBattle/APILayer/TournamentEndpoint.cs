﻿using SportExerciseBattle.DataLayer;
using SportExerciseBattle.HTTP;
using SportExerciseBattle.Models;
using System.Text.Json;
using HttpMethod = SportExerciseBattle.HTTP.HttpMethod;


namespace SportExerciseBattle.APILayer
{
    public class TournamentEndpoint : IHttpEndpoint
    {
        private TournamentDAO tournamentDAO = new TournamentDAO(); // Create an instance of TournamentDAO

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.GET)
            {
                GetTournamentInfo(rq, rs, tournamentDAO); // Delegate the task to GetStats method
                return true;
            }
            return false;
        }


        public void GetTournamentInfo(HttpRequest rq, HttpResponse rs, TournamentDAO tournamentDAO)
        {
            try
            {
                // Extrahieren des Tokens aus dem Authorization-Header
                var authHeader = rq.Headers.FirstOrDefault(h => h.Key.ToLower() == "authorization");
                if (authHeader.Key == null || !authHeader.Value.StartsWith("Basic "))
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Unauthorized";
                    return;
                }

                var token = authHeader.Value.Substring("Basic ".Length);
                var username = token.Split("-")[0]; // Extrahieren des Benutzernamens aus dem Token

                // Validieren des Tokens
                if (!TokenService.ValidateToken(token, username))
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Unauthorized";
                    return;
                }

                // Infos abrufen
                var tournament = Tournament.Instance;
                if (tournament.IsRunning == false) // Wenn kein Turnier läuft
                {
                    rs.ResponseCode = 404;
                    rs.Content = "currently none";
                    return;
                }

                // Teilnehmer und Führende in tournament schreiben, Infos serialisieren und zurückgeben
                tournamentDAO.GetParticipants();
                tournamentDAO.GetLeaders();
                rs.Content = JsonSerializer.Serialize(tournament);
                rs.Headers.Add("Content-Type", "application/json");
                rs.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                rs.ResponseCode = 500;
                rs.Content = $"Internal server error: {ex.Message}";
            }
        }
    }
}


