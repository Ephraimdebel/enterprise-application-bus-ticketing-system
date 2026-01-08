using System.Net.Mime;
using Passenger.Domain.Exceptions;

namespace Passenger.Api.AuthAndMiddleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsJsonAsync(new
            {
                type = "https://httpstatuses.com/403",
                title = "Forbidden",
                status = 403,
                detail = ex.Message
            });
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error: {Message}", ex.Message);
            await WriteProblemDetailsAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            await context.Response.WriteAsJsonAsync(new
            {
                type = "https://httpstatuses.com/500",
                title = "Unexpected error",
                status = 500,
                detail = "An unexpected error occurred."
            });
        }
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, DomainException ex)
    {
        var (status, title) = ex switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Validation error"),
            DuplicateEmailException => (StatusCodes.Status409Conflict, "Conflict"),
            PassengerAlreadyRegisteredException => (StatusCodes.Status409Conflict, "Conflict"),
            PassengerDeletedException => (StatusCodes.Status409Conflict, "Conflict"),
            PassengerNotFoundException => (StatusCodes.Status404NotFound, "Not found"),
            _ => (StatusCodes.Status400BadRequest, "Domain error")
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        return context.Response.WriteAsJsonAsync(new
        {
            type = $"https://httpstatuses.com/{status}",
            title,
            status,
            detail = ex.Message
        });
    }
}
