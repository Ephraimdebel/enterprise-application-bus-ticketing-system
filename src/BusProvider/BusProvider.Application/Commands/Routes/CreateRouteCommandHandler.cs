using BusProvider.Domain.Aggregates;
using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.Routes;

public sealed class CreateRouteCommandHandler : IRequestHandler<CreateRouteCommand, Guid>
{
    private readonly IRouteRepository _routeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRouteCommandHandler(IRouteRepository routeRepository, IUnitOfWork unitOfWork)
    {
        _routeRepository = routeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        var route = RouteAggregate.Create(request.BusId, request.Start, request.End, request.DistanceKm);
        await _routeRepository.AddAsync(route, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return route.Id;
    }
}
