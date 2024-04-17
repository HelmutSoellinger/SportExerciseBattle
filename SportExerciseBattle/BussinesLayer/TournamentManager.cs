﻿using System.Timers;
using SportExerciseBattle.Models;
using Timer = System.Timers.Timer;
using SportExerciseBattle.DataLayer;


public class TournamentManager
{
    private Timer tournamentTimer;
    private TournamentDAO tournamentDAO = new TournamentDAO();

    public TournamentManager()
    {
        tournamentTimer = new Timer(120000); // 2 minutes in milliseconds
        tournamentTimer.Elapsed += EndTournament;
    }

    public void StartTournament()
    {
        var tournament = Tournament.Instance;
        tournament.IsRunning = true;
        tournament.StartTime = DateTime.Now;

        tournamentTimer.Start();
    }

    private void EndTournament(object sender, ElapsedEventArgs e)
    {
        var tournament = Tournament.Instance;
        tournamentTimer.Stop();
        tournament.IsRunning = false;
        CalculateWinnerAndUpdateElo();
    }

    private void CalculateWinnerAndUpdateElo()
    {
        tournamentDAO.GetLeader();
        tournamentDAO.UpdateElo();
    }
}
