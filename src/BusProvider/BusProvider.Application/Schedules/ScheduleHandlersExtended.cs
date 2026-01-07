using BusProvider.Domain.Repositories;
using MediatR;

namespace BusProvider.Application.Schedules;

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

public sealed class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, bool>
{
    private readonly IScheduleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateScheduleCommandHandler(IScheduleRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (schedule is null)
        {
            return false;
        }

        schedule.Update(request.TripDate, request.Departure, request.Arrival, request.SeatsAvailable);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public sealed class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, bool>
{
    private readonly IScheduleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteScheduleCommandHandler(IScheduleRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (schedule is null)
        {
            return false;
        }

        await _repository.RemoveAsync(schedule, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
