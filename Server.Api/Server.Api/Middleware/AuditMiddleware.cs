using Microsoft.AspNetCore.Http;

namespace GovServices.Server.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Minimal implementation for compilation
        await _next(context);
    }
}
