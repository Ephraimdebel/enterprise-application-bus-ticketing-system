using MediatR;
using Passenger.Application.Abstractions;
using Passenger.Application.Commands.DeletePassenger;
using Passenger.Domain.Exceptions;
using Passenger.Domain.Repositories;

namespace Passenger.Application.Handlers.DeletePassenger;

public sealed class DeletePassengerHandler : IRequestHandler<DeletePassengerCommand, Unit>
{
    private readonly IPassengerRepository _repository;
    private readonly IDateTimeProvider _clock;

    public DeletePassengerHandler(IPassengerRepository repository, IDateTimeProvider clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public async Task<Unit> Handle(DeletePassengerCommand request, CancellationToken cancellationToken)
    {
        var passenger = await _repository.GetByIdAsync(request.PassengerId, cancellationToken)
            ?? throw new PassengerNotFoundException(request.PassengerId);

        passenger.SoftDelete(_clock.UtcNow);
        _repository.Update(passenger);
        await _repository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
