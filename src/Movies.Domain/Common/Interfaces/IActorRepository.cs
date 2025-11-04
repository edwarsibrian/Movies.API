using Movies.Domain.Entities;

namespace Movies.Domain.Common.Interfaces
{
    public interface IActorRepository
    {
        Task CreateAsync(Actor actor, CancellationToken cancellationToken);
    }
}
