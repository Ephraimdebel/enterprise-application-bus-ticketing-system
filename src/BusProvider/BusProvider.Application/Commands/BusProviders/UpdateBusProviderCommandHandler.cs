using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.BusProviders;

public sealed class UpdateBusProviderCommandHandler : IRequestHandler<UpdateBusProviderCommand, bool>
{
    private readonly IBusProviderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBusProviderCommandHandler(IBusProviderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateBusProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (provider is null)
        {
            return false;
        }

        provider.Update(request.Name, request.Email, request.PhoneNumber, request.Address);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
