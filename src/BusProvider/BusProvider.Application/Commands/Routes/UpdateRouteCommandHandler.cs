using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.Routes;

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
