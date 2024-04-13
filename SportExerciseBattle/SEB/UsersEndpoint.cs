using SportExerciseBattle.HTTP;
using System.Text.Json;
using HttpMethod = SportExerciseBattle.HTTP.HttpMethod;
using SportExerciseBattle.DataLayer;


namespace SportExerciseBattle.SEB
{
    public class UsersEndpoint : IHttpEndpoint
    {
        private UserDAO userDAO = new UserDAO(); // Create an instance of UserDAO
        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.POST)
            {
                CreateUser(rq, rs); // Delegate the task to UserDAO
                return true;
            }
            else if (rq.Method == HttpMethod.GET)
            {
                GetUsers(rq, rs);
                return true;
            }
            return false;
        }


        public void CreateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content ?? "");

                // call BL
                userDAO.CreateUser(rq, rs, user);

                rs.ResponseCode = 201;
                rs.ResponseMessage = "OK";
            }
            catch (Exception)
            {
                rs.ResponseCode = 400;
                rs.Content = "Failed to parse User data! ";
            }
        }

        public void GetUsers(HttpRequest rq, HttpResponse rs)
        {
            rs.Content = JsonSerializer.Serialize(new User[] { new User() { Username = "Max Muster", Password="1234" } });
            rs.Headers.Add("Content-Type", "application/json");
        }
    }
}
