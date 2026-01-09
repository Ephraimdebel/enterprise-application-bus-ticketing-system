using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.Schedules;

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
