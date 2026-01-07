using BusProvider.Domain.Repositories;
using MediatR;

namespace BusProvider.Application.Buses;

public sealed class ListBusesQueryHandler : IRequestHandler<ListBusesQuery, List<BusResponse>>
{
    private readonly IBusRepository _repository;

    public ListBusesQueryHandler(IBusRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BusResponse>> Handle(ListBusesQuery request, CancellationToken cancellationToken)
    {
        var buses = request.ProviderId.HasValue
            ? await _repository.GetByProviderAsync(request.ProviderId.Value, cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        return buses
            .Select(b => new BusResponse(b.Id, b.ProviderId, b.BusNumber.Value, b.BusType.Value, b.SeatCapacity.Value))
            .ToList();
    }
}

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

public sealed class DeleteBusCommandHandler : IRequestHandler<DeleteBusCommand, bool>
{
    private readonly IBusRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBusCommandHandler(IBusRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteBusCommand request, CancellationToken cancellationToken)
    {
        var bus = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (bus is null)
        {
            return false;
        }

        await _repository.RemoveAsync(bus, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
