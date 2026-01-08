namespace Trip.Application.DTOs;

public sealed record ScheduleDto(Guid Id, Guid BusId, Guid RouteId, DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable);
