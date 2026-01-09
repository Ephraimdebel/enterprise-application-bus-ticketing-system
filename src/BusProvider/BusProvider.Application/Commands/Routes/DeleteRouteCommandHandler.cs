using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Commands.Routes;

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
