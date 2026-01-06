using MediatR;
using Passenger.Application.Abstractions;
using Passenger.Application.Commands.RegisterPassenger;
using Passenger.Application.DTOs;
using Passenger.Application.Mappings;
using Passenger.Domain.Exceptions;
using Passenger.Domain.Aggregates;
using Passenger.Domain.Repositories;
using Passenger.Domain.ValueObjects;

namespace Passenger.Application.Handlers.RegisterPassenger;

public sealed class RegisterPassengerHandler : IRequestHandler<RegisterPassengerCommand, PassengerDto>
{
    private readonly IPassengerRepository _repository;
    private readonly IDateTimeProvider _clock;

    public RegisterPassengerHandler(IPassengerRepository repository, IDateTimeProvider clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public async Task<PassengerDto> Handle(RegisterPassengerCommand request, CancellationToken cancellationToken)
    {
        var name = Name.Create(request.FirstName, request.LastName);
        var email = Email.Create(request.Email);
        var phone = PhoneNumber.Create(request.CountryCode, request.PhoneNumber);

        var existing = await _repository.GetByIdAsync(request.PassengerId, cancellationToken);
        if (existing is not null)
            throw new PassengerAlreadyRegisteredException(request.PassengerId);

        var exists = await _repository.ExistsWithEmailAsync(email, excludingPassengerId: null, cancellationToken);
        var passenger = Passenger.Domain.Aggregates.Passenger.Register(
            id: request.PassengerId,
            name: name,
            email: email,
            phoneNumber: phone,
            emailIsUnique: !exists,
            utcNow: _clock.UtcNow);

        await _repository.AddAsync(passenger, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return passenger.ToDto();
    }
}
