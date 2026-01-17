namespace Colegio.Web.Infrastructure.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; }
    public ApiProblemModel? Problem { get; }

    public ApiException(string message, int statusCode, ApiProblemModel? problem = null)
        : base(message)
    {
        StatusCode = statusCode;
        Problem = problem;
    }
}