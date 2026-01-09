using BusProvider.Domain.Aggregates;
using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.BusProviders;

public sealed class RegisterBusProviderCommandHandler : IRequestHandler<RegisterBusProviderCommand, Guid>
{
    private readonly IBusProviderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterBusProviderCommandHandler(IBusProviderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(RegisterBusProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = BusProviderAggregate.Register(request.Name, request.Email, request.PhoneNumber, request.Address);

        await _repository.AddAsync(provider, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return provider.Id;
    }
}
