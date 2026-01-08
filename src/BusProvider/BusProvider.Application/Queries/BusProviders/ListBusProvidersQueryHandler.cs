using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Queries.BusProviders;

public sealed class ListBusProvidersQueryHandler : IRequestHandler<ListBusProvidersQuery, List<BusProviderResponse>>
{
    private readonly IBusProviderRepository _repository;

    public ListBusProvidersQueryHandler(IBusProviderRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BusProviderResponse>> Handle(ListBusProvidersQuery request, CancellationToken cancellationToken)
    {
        var providers = await _repository.GetAllAsync(cancellationToken);
        return providers
            .Select(p => new BusProviderResponse(p.Id, p.Name, p.Email.Value, p.ContactInfo.PhoneNumber, p.ContactInfo.Address))
            .ToList();
    }
}
