using Movies.Domain.Common.Interfaces;
using Movies.Domain.Entities;
using Movies.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Movies.Repository.Implementations
{
    public class ActorRepository : IActorRepository
    {
        private readonly APIMovieDbContext dbContext;

        public ActorRepository(APIMovieDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Actor actor, CancellationToken cancellationToken)
        {
            await dbContext.Actors.AddAsync(actor, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Actor?> GetActorByFileNameAsync(string fileName, CancellationToken cancellationToken)
        {
            return await dbContext.Actors
                .FirstOrDefaultAsync(a => a.Picture != null && a.Picture.Contains(fileName), cancellationToken);
        }
                
        public async Task<bool> UpdateAsync(Actor actor, CancellationToken cancellationToken)
        {
            bool exists = await dbContext.Actors.AnyAsync(a => a.Id == actor.Id, cancellationToken);
            if (!exists)
            {
                return false;
            }
            dbContext.Actors.Update(actor);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
