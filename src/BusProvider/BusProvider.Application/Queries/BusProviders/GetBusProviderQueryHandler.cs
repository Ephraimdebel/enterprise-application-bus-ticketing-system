using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Queries.BusProviders;

public sealed class GetBusProviderQueryHandler : IRequestHandler<GetBusProviderQuery, BusProviderResponse?>
{
    private readonly IBusProviderRepository _repository;

    public GetBusProviderQueryHandler(IBusProviderRepository repository)
    {
        _repository = repository;
    }

    public async Task<BusProviderResponse?> Handle(GetBusProviderQuery request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return provider is null
            ? null
            : new BusProviderResponse(provider.Id, provider.Name, provider.Email.Value, provider.ContactInfo.PhoneNumber, provider.ContactInfo.Address);
    }
}
