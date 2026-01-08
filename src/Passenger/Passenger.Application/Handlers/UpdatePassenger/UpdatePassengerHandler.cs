using MediatR;
using Passenger.Application.Abstractions;
using Passenger.Application.Commands.UpdatePassenger;
using Passenger.Application.DTOs;
using Passenger.Application.Mappings;
using Passenger.Domain.Exceptions;
using Passenger.Domain.Repositories;
using Passenger.Domain.ValueObjects;

namespace Passenger.Application.Handlers.UpdatePassenger;

public sealed class UpdatePassengerHandler : IRequestHandler<UpdatePassengerCommand, PassengerDto>
{
    private readonly IPassengerRepository _repository;
    private readonly IDateTimeProvider _clock;

    public UpdatePassengerHandler(IPassengerRepository repository, IDateTimeProvider clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public async Task<PassengerDto> Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
    {
        var passenger = await _repository.GetByIdAsync(request.PassengerId, cancellationToken)
            ?? throw new PassengerNotFoundException(request.PassengerId);

        var name = Name.Create(request.FirstName, request.LastName);
        var email = Email.Create(request.Email);
        var phone = PhoneNumber.Create(request.CountryCode, request.PhoneNumber);

        var emailExists = await _repository.ExistsWithEmailAsync(email, excludingPassengerId: request.PassengerId, cancellationToken);
        passenger.UpdateProfile(name, email, phone, emailIsUniqueIfChanged: !emailExists, utcNow: _clock.UtcNow);

        _repository.Update(passenger);
        await _repository.SaveChangesAsync(cancellationToken);

        return passenger.ToDto();
    }
}
