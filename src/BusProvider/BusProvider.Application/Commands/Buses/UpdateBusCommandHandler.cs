using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.Buses;

public sealed class UpdateBusCommandHandler : IRequestHandler<UpdateBusCommand, bool>
{
    private readonly IBusRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBusCommandHandler(IBusRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateBusCommand request, CancellationToken cancellationToken)
    {
        var bus = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (bus is null)
        {
            return false;
        }

        var exists = await _repository.ExistsWithNumberAsync(bus.ProviderId, request.BusNumber, request.Id, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("Bus number must be unique per provider.");
        }

        bus.Update(request.BusNumber, request.BusType, request.SeatCapacity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
