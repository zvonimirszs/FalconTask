using System.Net;
using System.Text.Json;

namespace Identity.Helpers;
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    // TO DO: logirati exception (DB ili file sistem)
    public async Task Invoke(HttpContext context)
    {
        Console.WriteLine($"--> Calling Invoke in ErrorHandlerMiddleware.");
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            Console.WriteLine($"--> Calling Invoke/Exception in ErrorHandlerMiddleware: {error}");

            switch (error)
            {
                case AppException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case AccessViolationException e:
                    // access error
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    // unhandled error
                    Console.WriteLine($"--> Calling Unhandled Exception in ErrorHandlerMiddleware: {error}");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}