using BusProvider.Domain.Aggregates;
using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.Buses;

public sealed class CreateBusCommandHandler : IRequestHandler<CreateBusCommand, Guid>
{
    private readonly IBusRepository _busRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBusCommandHandler(IBusRepository busRepository, IUnitOfWork unitOfWork)
    {
        _busRepository = busRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateBusCommand request, CancellationToken cancellationToken)
    {
        var exists = await _busRepository.ExistsWithNumberAsync(request.ProviderId, request.BusNumber, null, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("Bus number must be unique per provider.");
        }

        var bus = BusAggregate.Create(request.ProviderId, request.BusNumber, request.BusType, request.SeatCapacity);
        await _busRepository.AddAsync(bus, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return bus.Id;
    }
}
