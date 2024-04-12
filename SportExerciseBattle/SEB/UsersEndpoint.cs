using _08A3A4HttpServerDemo.HTTP;
using System.Text.Json;
using HttpMethod = _08A3A4HttpServerDemo.HTTP.HttpMethod;

namespace _08A3A4HttpServerDemo.SEB
{
    public class UsersEndpoint : IHttpEndpoint
    {
        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.POST)
            {
                CreateUser(rq, rs);
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
