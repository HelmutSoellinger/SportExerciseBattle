using SportExerciseBattle.HTTP;
using SportExerciseBattle.APILayer;
using System.Net;


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
