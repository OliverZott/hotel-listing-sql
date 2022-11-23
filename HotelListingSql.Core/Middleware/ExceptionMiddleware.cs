using HotelListingSql.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace HotelListingSql.Core.Middleware;

public class ExceptionMiddleware
{
    // Intercept the request
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;  // next embodies an incoming request (hijacks the request)
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);   // awaiting result of next request relative to the context
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, $"Something went wrong while processing {context.Request.Path}");
            await HandleExceptionAsync(context, exc);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";      // Set response-type for the response
        var statusCode = HttpStatusCode.InternalServerError;    // what for?

        var errorDetails = new ErrorDetails
        {
            ErrorType = "Failure",
            ErrorMessage = exception.Message,

        };

        switch (exception)
        {
            case MyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorDetails.ErrorType = "Not Found";
                break;
            default:
                break;
        }

        var response = JsonConvert.SerializeObject(errorDetails);
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(response);   // Write new response body to the context-response
    }
}

public class ErrorDetails
{
    public string ErrorType { get; set; }
    public string ErrorMessage { get; set; }
}