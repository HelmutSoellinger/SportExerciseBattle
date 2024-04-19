using System.Timers;
using SportExerciseBattle.Models;
using Timer = System.Timers.Timer;
using SportExerciseBattle.DataLayer;


public class TournamentManager
{
    public Timer TournamentTimer { get; private set; }
    private TournamentDAO tournamentDAO = new TournamentDAO();

    public TournamentManager()
    {
        TournamentTimer = new Timer(120000); // 2 minutes in milliseconds
        TournamentTimer.Elapsed += EndTournament;
    }

    public void StartTournament()
    {
        var tournament = Tournament.Instance;
        tournament.IsRunning = true;
        tournament.StartTime = DateTime.Now;

        TournamentTimer.Start();
    }

    public void EndTournament(object sender, ElapsedEventArgs e)
    {
        var tournament = Tournament.Instance;
        TournamentTimer.Stop();
        tournament.IsRunning = false;
        tournament.Log.Add(DateTime.Now + "The fight is over! Tournament ended!"); // Log the end of the tournament.
        CalculateWinnerAndUpdateElo();
        // Reset the tournament singleton for the next tournament.
        tournament.LeadingUsers.Clear();
        tournament.Participants.Clear();
    }

    private void CalculateWinnerAndUpdateElo()
    {
        tournamentDAO.GetParticipants();
        tournamentDAO.GetLeaders();
        tournamentDAO.UpdateElo();
        tournamentDAO.UpdateTournamentStats();
    }
}
