using Booking.Application.Interfaces;
using System.Net.Http.Json;

namespace Booking.Infrastructure.ExternalServices;

internal sealed class TripService : ITripService
{
    private readonly HttpClient _httpClient;

    public TripService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ExistsAsync(Guid tripId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Note: In a production environment with Keycloak, you would need to attach a Bearer token here
            var response = await _httpClient.GetAsync($"/trips/{tripId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
