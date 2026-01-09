namespace BusProvider.Application.DTOs;

public record RegisterBusProviderRequest(string Name, string Email, string PhoneNumber, string Address);
public record UpdateBusProviderRequest(string Name, string Email, string PhoneNumber, string Address);
public record CreateBusRequest(Guid ProviderId, string BusNumber, string BusType, int SeatCapacity);
public record UpdateBusRequest(string BusNumber, string BusType, int SeatCapacity);
public record CreateRouteRequest(Guid BusId, string Start, string End, double DistanceKm);
public record UpdateRouteRequest(string Start, string End, double DistanceKm);
public record CreateScheduleRequest(Guid BusId, Guid RouteId, DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable);
public record UpdateScheduleRequest(DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable);
