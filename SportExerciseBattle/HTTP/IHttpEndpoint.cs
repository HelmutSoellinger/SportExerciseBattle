
namespace SportExerciseBattle.HTTP
{
    public interface IHttpEndpoint
    {
        bool HandleRequest(HttpRequest rq, HttpResponse rs);
    }
}
