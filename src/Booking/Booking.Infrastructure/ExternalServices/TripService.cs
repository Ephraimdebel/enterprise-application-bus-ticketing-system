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
            var response = await _httpClient.GetAsync($"/api/trips/{tripId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
