using System.Timers;
using SportExerciseBattle.Models;
using Timer = System.Timers.Timer;


public class TournamentManager
{
    private Timer tournamentTimer;
    private Tournament currentTournament;

    public TournamentManager()
    {
        tournamentTimer = new Timer(120000); // 2 minutes in milliseconds
        tournamentTimer.Elapsed += EndTournament;
    }

    public void StartTournament()
    {
        //if Tournament is false, then start the tournament maybe in der datenbank abspeichern und dann abfragen
        currentTournament = new Tournament
        {
            IsRunning = true,
            StartTime = DateTime.Now,
            LeadingUser = "No participants yet"
        };

        tournamentTimer.Start();
    }

    private void EndTournament(object sender, ElapsedEventArgs e)
    {
        tournamentTimer.Stop();
        currentTournament.IsRunning = false;

        // Calculate the winner and update Elo ratings
        CalculateWinnerAndUpdateElo();
    }

    private void CalculateWinnerAndUpdateElo()
    {
        // Logic to calculate the winner and update Elo ratings
        // This will depend on how you're tracking participants and their scores
    }
}
