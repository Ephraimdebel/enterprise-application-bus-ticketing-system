using BusProvider.Domain.Repositories;
using MediatR;

namespace BusProvider.Application.Routes;

public sealed class ListRoutesQueryHandler : IRequestHandler<ListRoutesQuery, List<RouteResponse>>
{
    private readonly IRouteRepository _repository;

    public ListRoutesQueryHandler(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RouteResponse>> Handle(ListRoutesQuery request, CancellationToken cancellationToken)
    {
        var routes = request.BusId.HasValue
            ? await _repository.GetByBusAsync(request.BusId.Value, cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        return routes
            .Select(r => new RouteResponse(r.Id, r.BusId, r.Start.Value, r.End.Value, r.Distance.Kilometers))
            .ToList();
    }
}

public sealed class UpdateRouteCommandHandler : IRequestHandler<UpdateRouteCommand, bool>
{
    private readonly IRouteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRouteCommandHandler(IRouteRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
    {
        var route = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (route is null)
        {
            return false;
        }

        route.Update(request.Start, request.End, request.DistanceKm);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public sealed class DeleteRouteCommandHandler : IRequestHandler<DeleteRouteCommand, bool>
{
    private readonly IRouteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRouteCommandHandler(IRouteRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
        var route = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (route is null)
        {
            return false;
        }

        await _repository.RemoveAsync(route, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
