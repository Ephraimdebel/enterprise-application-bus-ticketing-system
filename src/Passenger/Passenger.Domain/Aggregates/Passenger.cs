using Passenger.Domain.Entities;    // PassengerId
using Passenger.Domain.Events;      // PassengerRegistered, PassengerUpdated
using Passenger.Domain.Exceptions;  // DuplicateEmailException, PassengerDeletedException
using Passenger.Domain.ValueObjects; // Name, Email, PhoneNumber, PassengerStatus

namespace Passenger.Domain.Aggregates
{
    /// <summary>
    /// Passenger Aggregate Root - owns identity/contact details and lifecycle status.
    /// All invariants must be enforced here.
    /// </summary>
    public sealed class Passenger  : AggregateRoot<PassengerId>
    {
        private Name _name = null!;
        private Email _email = null!;
        private PhoneNumber _phoneNumber = null!;
        private PassengerStatus _status = null!;

        // Constructor for creating a new Passenger
        private Passenger (PassengerId id) : base(id) { }

        // Parameterless constructor for ORM
        private Passenger () { }

        // Public getters
        public Name Name => _name;
        public Email Email => _email;
        public PhoneNumber PhoneNumber => _phoneNumber;
        public PassengerStatus Status => _status;

        public DateTime CreatedAtUtc { get; private set; }
        public DateTime UpdatedAtUtc { get; private set; }

        // Factory method to register a new passenger
        public static Passenger  Register(
            PassengerId id,
            Name name,
            Email email,
            PhoneNumber phoneNumber,
            bool emailIsUnique,
            DateTime utcNow)
        {
            if (!emailIsUnique)
                throw new DuplicateEmailException(email.Value);

            var passenger = new Passenger (id)
            {
                _name = name,
                _email = email,
                _phoneNumber = phoneNumber,
                _status = PassengerStatus.Active,
                CreatedAtUtc = utcNow,
                UpdatedAtUtc = utcNow
            };

            passenger.RaiseDomainEvent(new PassengerRegistered(
                passenger.Id,
                passenger.Name,
                passenger.Email,
                passenger.PhoneNumber,
                passenger.Status.Code,
                utcNow));

            return passenger;
        }

        public void UpdateProfile(
            Name name,
            Email email,
            PhoneNumber phoneNumber,
            bool emailIsUniqueIfChanged,
            DateTime utcNow)
        {
            EnsureNotDeleted();

            var emailChanged = !_email.Equals(email);
            if (emailChanged && !emailIsUniqueIfChanged)
                throw new DuplicateEmailException(email.Value);

            _name = name;
            _email = email;
            _phoneNumber = phoneNumber;
            UpdatedAtUtc = utcNow;

            RaiseDomainEvent(new PassengerUpdated(
                Id,
                Name,
                Email,
                PhoneNumber,
                Status.Code,
                utcNow));
        }

        public void Suspend(DateTime utcNow)
        {
            EnsureNotDeleted();
            if (_status.Code == PassengerStatusCode.Suspended) return;

            _status = PassengerStatus.Suspended;
            UpdatedAtUtc = utcNow;

            RaiseDomainEvent(new PassengerUpdated(Id, Name, Email, PhoneNumber, Status.Code, utcNow));
        }

        public void Activate(DateTime utcNow)
        {
            EnsureNotDeleted();
            if (_status.Code == PassengerStatusCode.Active) return;

            _status = PassengerStatus.Active;
            UpdatedAtUtc = utcNow;

            RaiseDomainEvent(new PassengerUpdated(Id, Name, Email, PhoneNumber, Status.Code, utcNow));
        }

        public void SoftDelete(DateTime utcNow)
        {
            if (_status.IsDeleted) return;

            _status = PassengerStatus.Deleted;
            UpdatedAtUtc = utcNow;

            RaiseDomainEvent(new PassengerUpdated(Id, Name, Email, PhoneNumber, Status.Code, utcNow));
        }

        private void EnsureNotDeleted()
        {
            if (_status.IsDeleted)
                throw new PassengerDeletedException();
        }
    }
}
