using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.BusProviders;

public sealed class DeleteBusProviderCommandHandler : IRequestHandler<DeleteBusProviderCommand, bool>
{
    private readonly IBusProviderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBusProviderCommandHandler(IBusProviderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteBusProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (provider is null)
        {
            return false;
        }

        await _repository.RemoveAsync(provider, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
