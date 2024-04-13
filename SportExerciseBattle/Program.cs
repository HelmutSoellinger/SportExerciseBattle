using SportExerciseBattle.HTTP;
using SportExerciseBattle.SEB;
using Npgsql;
using SportExerciseBattle.Data_Layer;
using System.Net;

Console.WriteLine("Our first simple HTTP-Server: http://localhost:10001/");

// ===== I. Start the HTTP-Server =====
HttpServer httpServer = new HttpServer(IPAddress.Any, 10001);
httpServer.RegisterEndpoint("users", new UsersEndpoint());
httpServer.RegisterEndpoint("sessions", new SessionsEndpoint());

/*DatabaseConnection.GetConnection();
Console.WriteLine("Database connection established!");
using var cmd = new NpgsqlCommand(@"INSERT INTO ""person""(username, password) VALUES (@username, @password)", DatabaseConnection.GetConnection());
cmd.Parameters.AddWithValue("username", "Max Mustermann");
cmd.Parameters.AddWithValue("password", "1234");
var affectedrows = cmd.ExecuteNonQuery();
Console.WriteLine($"Affected Rows: {affectedrows}");
*/
httpServer.Run();
// CREATE TABLE IF NOT EXISTS user (id INT PRIMARY KEY, username VARCHAR(50) NOT NULL, password VARCHAR(50