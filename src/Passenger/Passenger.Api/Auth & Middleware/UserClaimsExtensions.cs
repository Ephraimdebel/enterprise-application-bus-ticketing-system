using System.Security.Claims;
using System.Text.Json;
using Passenger.Domain.Entities;

namespace Passenger.Api.AuthAndMiddleware;

public static class UserClaimsExtensions
{
    public static PassengerId GetPassengerIdOrThrow(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
        if (!PassengerId.TryParse(sub, out var pid))
            throw new InvalidOperationException("Authenticated token does not contain a valid 'sub' (GUID) claim.");

        return pid;
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        // Keycloak commonly provides roles in realm_access.roles (JSON)
        if (user.IsInRole("admin")) return true;

        var realmAccessJson = user.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccessJson)) return false;

        try
        {
            using var doc = JsonDocument.Parse(realmAccessJson);
            if (!doc.RootElement.TryGetProperty("roles", out var roles)) return false;
            foreach (var role in roles.EnumerateArray())
            {
                if (string.Equals(role.GetString(), "admin", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }
        catch
        {
            // ignore parsing errors
        }

        return false;
    }
}
