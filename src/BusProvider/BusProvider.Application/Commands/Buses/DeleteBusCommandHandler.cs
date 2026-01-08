using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.Buses;

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
