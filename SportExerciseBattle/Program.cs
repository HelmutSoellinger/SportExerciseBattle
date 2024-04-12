using _08A3A4HttpServerDemo.HTTP;
using _08A3A4HttpServerDemo.SEB;
using Npgsql;
using SportExerciseBattle.Data_Layer;
using System.Net;

Console.WriteLine("Our first simple HTTP-Server: http://localhost:10001/");

// ===== I. Start the HTTP-Server =====
HttpServer httpServer = new HttpServer(IPAddress.Any, 10001);
httpServer.RegisterEndpoint("users", new UsersEndpoint());

DatabaseConnection.GetConnection();
Console.WriteLine("Database connection established!");
using var cmd = new NpgsqlCommand(@"INSERT INTO ""User""(username, password) VALUES (@username, @password)", DatabaseConnection.GetConnection());
cmd.Parameters.AddWithValue("username", "Max Muster");
cmd.Parameters.AddWithValue("password", "1234");
var affectedrows = cmd.ExecuteNonQuery();
Console.WriteLine($"Affected Rows: {affectedrows}");
httpServer.Run();