using Movies.Domain.Common.Interfaces;
using Movies.Domain.Entities;
using Movies.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Movies.Repository.Implementations
{
    public class ActorRepository : IActorRepository
    {
        private readonly APIMovieDbContext _dbContext;

        public ActorRepository(APIMovieDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Actor actor, CancellationToken cancellationToken)
        {
            await _dbContext.Actors.AddAsync(actor, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Actor?> GetActorByFileNameAsync(string fileName, CancellationToken cancellationToken)
        {
            return await _dbContext.Actors
                .FirstOrDefaultAsync(a => a.Picture != null && a.Picture.Contains(fileName), cancellationToken);
        }
                
        public async Task<bool> UpdateAsync(Actor actor, CancellationToken cancellationToken)
        {
            bool exists = await _dbContext.Actors.AnyAsync(a => a.Id == actor.Id, cancellationToken);
            if (!exists)
            {
                return false;
            }
            _dbContext.Actors.Update(actor);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task UpdatePictureAsync(int actorId, string pictureUrl, CancellationToken cancellationToken)
        {
            await _dbContext.Actors
                .Where(a => a.Id == actorId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(a => a.Picture, pictureUrl),
                    cancellationToken);
        }
    }
}
