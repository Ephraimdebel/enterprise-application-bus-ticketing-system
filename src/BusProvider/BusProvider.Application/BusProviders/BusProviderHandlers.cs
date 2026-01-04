using BusProvider.Domain.Repositories;
using MediatR;

namespace BusProvider.Application.BusProviders;

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
