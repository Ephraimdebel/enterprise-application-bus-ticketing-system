using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Queries.Schedules;

public sealed class GetScheduleQueryHandler : IRequestHandler<GetScheduleQuery, ScheduleResponse?>
{
    private readonly IScheduleRepository _repository;

    public GetScheduleQueryHandler(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<ScheduleResponse?> Handle(GetScheduleQuery request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return schedule is null
            ? null
            : new ScheduleResponse(schedule.Id, schedule.BusId, schedule.RouteId, schedule.TripDate.Value, schedule.Departure.Value, schedule.Arrival.Value, schedule.SeatsAvailable.Value);
    }
}
