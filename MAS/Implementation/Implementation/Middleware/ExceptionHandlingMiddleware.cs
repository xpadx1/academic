using System.Net;
using System.Text.Json;
using Implementation.Application.Exceptions;

namespace Implementation.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message),
            ConflictException => (HttpStatusCode.Conflict, exception.Message),
            BusinessRuleException => (HttpStatusCode.UnprocessableEntity, exception.Message),
            PaymentFailedException => (HttpStatusCode.PaymentRequired, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception while processing request {Path}", context.Request.Path);
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var error = new
        {
            status = (int)statusCode,
            title = statusCode.ToString(),
            detail = message,
            traceId = context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(error);
        await context.Response.WriteAsync(json);
    }
}