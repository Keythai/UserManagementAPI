using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Token is missing.");
            return;
        }

        if (!ValidateToken(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid token.");
            return;
        }

        await _next(context);
    }

    private bool ValidateToken(string token)
    {
        // Implement your token validation logic here
        // For example, check if the token is in a list of valid tokens
        return token == "valid-token"; // Replace with your actual validation logic
    }
}