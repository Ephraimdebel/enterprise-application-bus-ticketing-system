using BusProvider.Domain.Aggregates;
using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.Schedules;

public sealed class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, Guid>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateScheduleCommandHandler(IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork)
    {
        _scheduleRepository = scheduleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = ScheduleAggregate.Create(request.BusId, request.RouteId, request.TripDate, request.Departure, request.Arrival, request.SeatsAvailable);
        await _scheduleRepository.AddAsync(schedule, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return schedule.Id;
    }
}
