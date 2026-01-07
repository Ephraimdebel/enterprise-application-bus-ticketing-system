using MediatR;
using global::Booking.Domain;

namespace Booking.Application;

public interface ICommand : IRequest<Guid>
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Guid>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}
