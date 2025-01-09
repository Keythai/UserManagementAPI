using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the request method and path
        _logger.LogInformation("HTTP Request: {Method} {Path}", context.Request.Method, context.Request.Path);

        // Call the next middleware in the pipeline
        await _next(context);

        // Log the response status code
        _logger.LogInformation("HTTP Response: {StatusCode}", context.Response.StatusCode);
    }
}