using System.Net.Mime;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BusProvider.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error: {Message}", ex.Message);
            await WriteProblem(context, StatusCodes.Status400BadRequest, "Validation error", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation: {Message}", ex.Message);
            await WriteProblem(context, StatusCodes.Status400BadRequest, "Invalid operation", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error");
            await WriteProblem(context, StatusCodes.Status500InternalServerError, "Unexpected error", "An unexpected error occurred.");
        }
    }

    private static Task WriteProblem(HttpContext ctx, int status, string title, string detail)
    {
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = MediaTypeNames.Application.Json;
        return ctx.Response.WriteAsJsonAsync(new
        {
            type = $"https://httpstatuses.com/{status}",
            title,
            status,
            detail
        });
    }
}
