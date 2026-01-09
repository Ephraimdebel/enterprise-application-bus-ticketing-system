using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Queries.Schedules;

public sealed class ListSchedulesQueryHandler : IRequestHandler<ListSchedulesQuery, List<ScheduleResponse>>
{
    private readonly IScheduleRepository _repository;

    public ListSchedulesQueryHandler(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ScheduleResponse>> Handle(ListSchedulesQuery request, CancellationToken cancellationToken)
    {
        var schedules = request.RouteId.HasValue
            ? await _repository.GetByRouteAsync(request.RouteId.Value, cancellationToken)
            : request.BusId.HasValue
                ? await _repository.GetByBusAsync(request.BusId.Value, cancellationToken)
                : await _repository.GetAllAsync(cancellationToken);

        return schedules
            .Select(s => new ScheduleResponse(s.Id, s.BusId, s.RouteId, s.TripDate.Value, s.Departure.Value, s.Arrival.Value, s.SeatsAvailable.Value))
            .ToList();
    }
}
