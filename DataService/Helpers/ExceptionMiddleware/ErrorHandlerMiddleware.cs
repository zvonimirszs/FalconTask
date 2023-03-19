namespace DataService.Helpers.ExceptionMiddleware;

using System.Net;
using System.Text.Json;
using DataService.HelpersExceptionMiddleware.Exceptions;
using DataService.Models;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        Console.WriteLine($"--> Calling ErrorHandlerMiddleware");
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"--> Calling Invoke in ErrorHandlerMiddleware");
        try
        {
            await _next(context);
        }
        catch (AppException appEx)
        {
            //POBOLJŠANJA: logirati exception (DB ili file sistem)
            await HandleExceptionAsync(context, appEx);
        }
        catch (ServiceException srvEx)
        {
            //POBOLJŠANJA: logirati exception (DB ili file sistem)
            await HandleExceptionAsync(context, srvEx);
        }
        catch (AccessViolationException avEx)
        {
            //POBOLJŠANJA: logirati exception (DB ili file sistem)
            await HandleExceptionAsync(context, avEx);
        }
        catch (Exception error)
        {
            //POBOLJŠANJA: logirati exception (DB ili file sistem)
            Console.WriteLine($"--> Calling Invoke/Exception in ErrorHandlerMiddleware: {error}");
            await HandleExceptionAsync(context, error);
        }
    }

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";        

        var statusCode = exception switch
        {
            AccessViolationException =>  (int)HttpStatusCode.Unauthorized,
            AppException =>  (int)HttpStatusCode.BadRequest,
            ServiceException =>  (int)HttpStatusCode.ServiceUnavailable,
            _ => (int)HttpStatusCode.InternalServerError
            
        };
        context.Response.StatusCode = statusCode;

        var message = exception switch
        {
            AccessViolationException =>  "Access violation error from the custom middleware",
            AppException =>  "AppException error from the custom middleware",
            ServiceException =>  exception.Message,
            _ => "Internal Server Error from the custom middleware."
            
        };

        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = message
        }.ToString());
    }

}