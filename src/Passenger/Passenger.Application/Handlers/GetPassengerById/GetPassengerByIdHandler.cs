using MediatR;
using Passenger.Application.DTOs;
using Passenger.Application.Mappings;
using Passenger.Application.Queries.GetPassengerById;
using Passenger.Domain.Repositories;

namespace Passenger.Application.Handlers.GetPassengerById;

public sealed class GetPassengerByIdHandler : IRequestHandler<GetPassengerByIdQuery, PassengerDto?>
{
    private readonly IPassengerRepository _repository;

    public GetPassengerByIdHandler(IPassengerRepository repository)
    {
        _repository = repository;
    }

    public async Task<PassengerDto?> Handle(GetPassengerByIdQuery request, CancellationToken cancellationToken)
    {
        var passenger = await _repository.GetByIdAsync(request.PassengerId, cancellationToken);
        return passenger?.ToDto();
    }
}
