namespace Trip.Application.DTOs;

public sealed record RouteDto(Guid Id, Guid BusId, string Start, string End, double DistanceKm);
