using SportExerciseBattle.HTTP;
using SportExerciseBattle.APILayer;
using SportExerciseBattle.DataLayer;

using System.Net;
using SportExerciseBattle.Models;

Console.WriteLine("Our first simple HTTP-Server: http://localhost:10001/");

// ===== I. Start the HTTP-Server =====
HttpServer httpServer = new HttpServer(IPAddress.Any, 10001);
//Start Endpoints
httpServer.RegisterEndpoint("users", new UsersEndpoint());
httpServer.RegisterEndpoint("sessions", new SessionsEndpoint());
httpServer.RegisterEndpoint("stats", new StatsEndpoint());
httpServer.RegisterEndpoint("score", new ScoresEndpoint());
httpServer.RegisterEndpoint("history", new HistoryEndpoint());
httpServer.RegisterEndpoint("tournament", new TournamentEndpoint());
httpServer.RegisterEndpoint("ratio", new TournamentStatsEndpoint());
//Run the server
httpServer.Run();
/*
static void Main(string[] args)
{
    try
    {
        // Initialize the Tournament singleton with example data
        Tournament.Instance.StartTime = DateTime.Now.AddMinutes(-1); // Example start time
        Tournament.Instance.Participants = new List<string> { "Alice", "Bob", "Charlie", "David", "Eve" }; // Example participants
        /Tournament.Instance.LeadingUsers = new List<string> { "Alice", "Bob" }; // Example leading users

        // Create an instance of TournamentDAO
        var tournamentDAO = new TournamentDAO();

        // Test the GetParticipants method
        Console.WriteLine("Getting participants...");
        tournamentDAO.GetParticipants();
        Console.WriteLine($"Participants count: {Tournament.Instance.Participants.Count}");

       /* // Test the GetLeaders method
        Console.WriteLine("Getting leaders...");
        tournamentDAO.GetLeaders();
        Console.WriteLine($"Leading users count: {Tournament.Instance.LeadingUsers.Count}");
       
        // Test the UpdateElo method
        Console.WriteLine("Updating Elo...");
        tournamentDAO.UpdateElo();

        / Test the UpdateTournamentStats method
        Console.WriteLine("Updating tournament stats...");
        tournamentDAO.UpdateTournamentStats();
        
        Console.WriteLine("Debug completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}
*/