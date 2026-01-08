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
            // Note: The Passenger API endpoints require authorization. 
            // Internal module communication may require a service-to-service token.
            var response = await _httpClient.GetAsync($"/passengers/{passengerId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<PassengerResponse?> GetByIdAsync(Guid passengerId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/passengers/{passengerId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<PassengerResponse>(cancellationToken: cancellationToken);
        }
        catch
        {
            return null;
        }
    }
}
