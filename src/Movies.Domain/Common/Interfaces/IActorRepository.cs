using Movies.Domain.Entities;

namespace Movies.Domain.Common.Interfaces
{
    public interface IActorRepository
    {
        Task CreateAsync(Actor actor, CancellationToken cancellationToken);
        Task<Actor?> GetActorByFileNameAsync(string fileName, CancellationToken cancellationToken);
        Task UpdatePictureAsync(int actorId, string pictureUrl, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Actor actor, CancellationToken cancellationToken);
    }
}
