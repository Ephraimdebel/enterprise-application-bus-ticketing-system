using MediatR;
using global::Booking.Domain;

namespace Booking.Application;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
