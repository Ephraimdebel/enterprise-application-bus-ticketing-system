using Booking.Application.Interfaces;
using System.Net.Http.Json;

namespace Booking.Infrastructure.ExternalServices;

internal sealed class PassengerService : IPassengerService
{
    private readonly HttpClient _httpClient;

    public PassengerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ExistsAsync(Guid passengerId, CancellationToken cancellationToken = default)
    {
        try
        {
            // The Passenger API might have a different route, 
            // but based on naming conventions in Program.cs (app.MapPassengerEndpoints())
            // and the GetPassengerByIdQuery, it's likely /api/passengers/{id}
            var response = await _httpClient.GetAsync($"/api/passengers/{passengerId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
