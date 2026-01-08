using MediatR;
using Rating.Application.Interfaces;
using RatingEntity = Rating.Domain.Aggregates.Rating;
using Rating.Domain.ValueObjects;

namespace Rating.Application.Commands.CreateRating;

public sealed class CreateRatingCommandHandler
    : IRequestHandler<CreateRatingCommand, Guid>
{
    private readonly IRatingRepository _repository;

    public CreateRatingCommandHandler(IRatingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(
        CreateRatingCommand request,
        CancellationToken cancellationToken
    )
    {
        var rating = new RatingEntity(
            Guid.NewGuid(),
            request.TripId,
            request.UserId,
            request.TargetId,
            new Score(request.Stars),
            request.Comment
        );

        await _repository.AddAsync(rating, cancellationToken);

        return rating.Id;
    }
}
