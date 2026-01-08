using System.Net.Http.Json;
using System.Text.Json;
using Trip.Application.DTOs;
using Trip.Application.Interfaces;
using Trip.Infrastructure.Serialization;

namespace Trip.Infrastructure.Clients;

public sealed class BusProviderClient : IBusProviderGateway
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public BusProviderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        _jsonOptions.Converters.Add(new DateOnlyJsonConverter());
        _jsonOptions.Converters.Add(new TimeOnlyJsonConverter());
    }

    public async Task<BusDto?> GetBusAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<BusDto>($"/buses/{busId}", _jsonOptions, cancellationToken);
    }

    public async Task<IReadOnlyList<RouteDto>> ListRoutesAsync(Guid? busId, CancellationToken cancellationToken = default)
    {
        var query = busId.HasValue ? $"?busId={busId}" : string.Empty;
          var result = await _httpClient.GetFromJsonAsync<List<RouteDto>>($"/routes{query}", _jsonOptions, cancellationToken);
          return result ?? new List<RouteDto>();
    }

    public async Task<IReadOnlyList<ScheduleDto>> ListSchedulesAsync(Guid? busId, Guid? routeId, CancellationToken cancellationToken = default)
    {
        var queryParts = new List<string>();
        if (busId.HasValue) queryParts.Add($"busId={busId}");
        if (routeId.HasValue) queryParts.Add($"routeId={routeId}");
        var query = queryParts.Count > 0 ? "?" + string.Join("&", queryParts) : string.Empty;

        var result = await _httpClient.GetFromJsonAsync<List<ScheduleDto>>($"/schedules{query}", _jsonOptions, cancellationToken);
          return result ?? new List<ScheduleDto>();
    }
}
